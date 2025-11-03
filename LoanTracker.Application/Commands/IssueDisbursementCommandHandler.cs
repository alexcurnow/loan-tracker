using LoanTracker.Application.Common;
using LoanTracker.Application.Interfaces;
using LoanTracker.Domain.Entities;
using LoanTracker.Domain.Enums;
using LoanTracker.Domain.Events;
using LoanTracker.Domain.Interfaces;
using LoanTracker.Domain.Services;
using LoanTracker.Domain.ValueObjects;

namespace LoanTracker.Application.Commands;

public class IssueDisbursementCommandHandler
    : ICommandHandler<IssueDisbursementCommand, Result<DisbursementDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ILoanRepository _loanRepository;
    private readonly IEventStore _eventStore;
    private readonly WorkflowStateMachine _workflowStateMachine;

    public IssueDisbursementCommandHandler(
        IProjectRepository projectRepository,
        ILoanRepository loanRepository,
        IEventStore eventStore,
        WorkflowStateMachine workflowStateMachine)
    {
        _projectRepository = projectRepository;
        _loanRepository = loanRepository;
        _eventStore = eventStore;
        _workflowStateMachine = workflowStateMachine;
    }

    public async Task<Result<DisbursementDto>> HandleAsync(IssueDisbursementCommand command)
    {
        // 1. Validate project exists
        var project = await _projectRepository.GetByIdAsync(command.ProjectId);
        if (project == null)
        {
            return Error.NotFound($"Project with ID {command.ProjectId} not found");
        }

        // 2. Validate loan exists and is in appropriate status
        var loan = await _loanRepository.GetByIdAsync(project.LoanId);
        if (loan == null)
        {
            return Result<DisbursementDto>.Failure(
                Error.NotFound($"Loan with ID {project.LoanId} not found")
            );
        }

        if (loan.Status != LoanStatus.Approved && loan.Status != LoanStatus.Construction)
        {
            return Result<DisbursementDto>.Failure(
                Error.Validation($"Cannot issue disbursement for loan in {loan.Status} status. Loan must be Approved or Construction.")
            );
        }

        // 3. Create Money value object
        var amount = Money.FromDecimal(command.Amount, command.Currency);

        // 4. Generate disbursement ID
        var disbursementId = Guid.NewGuid();
        var occurredAt = DateTime.UtcNow;

        // 5. Create and append DisbursementIssued event (this is now the source of truth!)
        var disbursementEvent = new DisbursementIssued
        {
            DisbursementId = disbursementId,
            LoanId = loan.LoanId,
            ProjectId = project.ProjectId,
            Amount = amount,
            DisbursementDate = command.DisbursementDate,
            RecipientName = command.RecipientName,
            RecipientDetails = command.RecipientDetails,
            IssuedBy = command.IssuedBy
        };

        await _eventStore.AppendToStreamAsync(loan.LoanId, disbursementEvent);

        // 6. Check if this is first disbursement → transition to Construction
        // Query event stream to count disbursements
        var streamEvents = await _eventStore.GetStreamEventsAsync(loan.LoanId);
        var disbursementCount = streamEvents.Count(e => e is DisbursementIssued);
        var isFirstDisbursement = disbursementCount == 1;

        if (loan.Status == LoanStatus.Approved && isFirstDisbursement)
        {
            var oldStatus = loan.Status;
            loan.Status = LoanStatus.Construction;
            loan.UpdatedAt = DateTime.UtcNow;
            await _loanRepository.UpdateAsync(loan);

            // Publish LoanStatusChanged event
            var statusChangedEvent = new LoanStatusChanged
            {
                LoanId = loan.LoanId,
                FromStatus = oldStatus,
                ToStatus = LoanStatus.Construction,
                Reason = "First disbursement issued",
                ChangedBy = command.IssuedBy
            };

            await _eventStore.AppendToStreamAsync(loan.LoanId, statusChangedEvent);
        }

        // 7. Save all events (Marten projections will auto-update!)
        await _eventStore.SaveChangesAsync();

        // 8. Return DTO
        var dto = new DisbursementDto
        {
            DisbursementId = disbursementId,
            ProjectId = command.ProjectId,
            LoanId = loan.LoanId,
            Amount = amount.Amount,
            Currency = amount.Currency,
            DisbursementDate = command.DisbursementDate,
            RecipientName = command.RecipientName,
            RecipientDetails = command.RecipientDetails,
            CreatedAt = occurredAt,
            IsBackdated = command.DisbursementDate < occurredAt.Date
        };

        return dto;
    }
}

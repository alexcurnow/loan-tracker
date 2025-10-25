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
    private readonly IDisbursementRepository _disbursementRepository;
    private readonly IEventStore _eventStore;
    private readonly WorkflowStateMachine _workflowStateMachine;

    public IssueDisbursementCommandHandler(
        IProjectRepository projectRepository,
        ILoanRepository loanRepository,
        IDisbursementRepository disbursementRepository,
        IEventStore eventStore,
        WorkflowStateMachine workflowStateMachine)
    {
        _projectRepository = projectRepository;
        _loanRepository = loanRepository;
        _disbursementRepository = disbursementRepository;
        _eventStore = eventStore;
        _workflowStateMachine = workflowStateMachine;
    }

    public async Task<Result<DisbursementDto>> HandleAsync(IssueDisbursementCommand command)
    {
        // 1. Validate project exists
        var project = await _projectRepository.GetByIdAsync(command.ProjectId);
        if (project == null)
        {
            return Result<DisbursementDto>.Failure(
                Error.NotFound($"Project with ID {command.ProjectId} not found")
            );
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

        // 4. Create immutable Disbursement
        var disbursement = Disbursement.Create(
            projectId: command.ProjectId,
            amount: amount,
            disbursementDate: command.DisbursementDate,
            recipientName: command.RecipientName,
            recipientDetails: command.RecipientDetails
        );

        // 5. Save to database
        await _disbursementRepository.AddAsync(disbursement);

        // 6. Publish DisbursementIssued event
        var disbursementEvent = new DisbursementIssued
        {
            DisbursementId = disbursement.DisbursementId,
            LoanId = loan.LoanId,
            ProjectId = project.ProjectId,
            Amount = amount,
            DisbursementDate = command.DisbursementDate,
            RecipientName = command.RecipientName,
            RecipientDetails = command.RecipientDetails,
            IssuedBy = command.IssuedBy
        };

        await _eventStore.AppendToStreamAsync(loan.LoanId, disbursementEvent);

        // 7. Check if this is first disbursement → transition to Construction
        if (loan.Status == LoanStatus.Approved)
        {
            var existingDisbursements = await _disbursementRepository.GetByLoanIdAsync(loan.LoanId);
            var isFirstDisbursement = existingDisbursements.Count() == 1; // Only the one we just added

            if (isFirstDisbursement)
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
        }

        // 8. Save all events
        await _eventStore.SaveChangesAsync();

        // 9. Return DTO
        var dto = new DisbursementDto
        {
            DisbursementId = disbursement.DisbursementId,
            ProjectId = disbursement.ProjectId,
            LoanId = loan.LoanId,
            Amount = disbursement.AmountValue,
            Currency = disbursement.AmountCurrency,
            DisbursementDate = disbursement.DisbursementDate,
            RecipientName = disbursement.RecipientName,
            RecipientDetails = disbursement.RecipientDetails,
            CreatedAt = disbursement.CreatedAt,
            IsBackdated = disbursement.DisbursementDate < disbursement.CreatedAt.Date
        };

        return Result<DisbursementDto>.Success(dto);
    }
}

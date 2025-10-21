using LoanTracker.Application.Common;
using LoanTracker.Application.DTOs;
using LoanTracker.Application.Interfaces;
using LoanTracker.Domain.Enums;
using LoanTracker.Domain.Interfaces;
using LoanTracker.Domain.Services;

namespace LoanTracker.Application.Commands;

public record TransitionLoanStatusCommand(
    Guid LoanId,
    LoanStatus ToStatus,
    string? ReviewerNotes = null
);

public class TransitionLoanStatusCommandHandler : ICommandHandler<TransitionLoanStatusCommand, Result<LoanDto>>
{
    private readonly ILoanRepository _loanRepository;
    private readonly WorkflowStateMachine _stateMachine;

    public TransitionLoanStatusCommandHandler(ILoanRepository loanRepository, WorkflowStateMachine stateMachine)
    {
        _loanRepository = loanRepository;
        _stateMachine = stateMachine;
    }

    public async Task<Result<LoanDto>> HandleAsync(TransitionLoanStatusCommand command)
    {
        try
        {
            var loan = await _loanRepository.GetByIdAsync(command.LoanId);
            if (loan == null)
            {
                return Result<LoanDto>.Failure(Error.NotFound($"Loan with ID {command.LoanId} not found"));
            }

            // Validate the transition
            if (!_stateMachine.IsValidTransition(loan.Status, command.ToStatus))
            {
                var errorMessage = _stateMachine.GetTransitionErrorMessage(loan.Status, command.ToStatus);
                return Result<LoanDto>.Failure(Error.Validation(errorMessage));
            }

            // Update the loan status
            loan.Status = command.ToStatus;

            // Set decision date for terminal states
            if (command.ToStatus == LoanStatus.Approved || command.ToStatus == LoanStatus.Denied)
            {
                loan.DecisionDate = DateTime.UtcNow;
            }

            // Set reviewer notes if provided
            if (!string.IsNullOrWhiteSpace(command.ReviewerNotes))
            {
                loan.ReviewerNotes = command.ReviewerNotes;
            }

            var updatedLoan = await _loanRepository.UpdateAsync(loan);

            var loanDto = new LoanDto
            {
                LoanId = updatedLoan.LoanId,
                BorrowerName = updatedLoan.BorrowerName,
                BorrowerTypeId = updatedLoan.BorrowerTypeId,
                BorrowerTypeName = updatedLoan.BorrowerType?.TypeName ?? string.Empty,
                ContactPerson = updatedLoan.ContactPerson,
                ContactEmail = updatedLoan.ContactEmail,
                Amount = updatedLoan.Amount,
                InterestRate = updatedLoan.InterestRate,
                TermYears = updatedLoan.TermYears,
                Purpose = updatedLoan.Purpose,
                ApplicationDate = updatedLoan.ApplicationDate,
                Status = updatedLoan.Status,
                DecisionDate = updatedLoan.DecisionDate,
                ReviewerNotes = updatedLoan.ReviewerNotes,
                CreatedAt = updatedLoan.CreatedAt,
                UpdatedAt = updatedLoan.UpdatedAt
            };

            return Result<LoanDto>.Success(loanDto);
        }
        catch (Exception ex)
        {
            return Result<LoanDto>.Failure(Error.ServerError($"Failed to transition loan status: {ex.Message}"));
        }
    }
}

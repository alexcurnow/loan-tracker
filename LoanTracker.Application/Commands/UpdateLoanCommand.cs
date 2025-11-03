using LoanTracker.Application.DTOs;
using LoanTracker.Application.Interfaces;
using LoanTracker.Domain.Enums;
using LoanTracker.Domain.Interfaces;
using LoanTracker.Domain.Types;

namespace LoanTracker.Application.Commands;

public record UpdateLoanCommand(
    Guid LoanId,
    string BorrowerName,
    int BorrowerTypeId,
    string ContactPerson,
    string ContactEmail,
    decimal Amount,
    decimal InterestRate,
    TermYears TermYears,
    string Purpose
);

public class UpdateLoanCommandHandler : ICommandHandler<UpdateLoanCommand, LoanDto>
{
    private readonly ILoanRepository _loanRepository;

    public UpdateLoanCommandHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<LoanDto> HandleAsync(UpdateLoanCommand command)
    {
        var loan = await _loanRepository.GetByIdAsync(command.LoanId);
        if (loan == null)
        {
            throw new InvalidOperationException($"Loan with ID {command.LoanId} not found.");
        }

        // Only allow updates if the loan is in Open status
        if (loan.Status != LoanStatus.Open)
        {
            throw new InvalidOperationException($"Cannot update loan in {loan.Status} status. Only Open loans can be edited.");
        }

        loan.BorrowerName = command.BorrowerName;
        loan.BorrowerTypeId = command.BorrowerTypeId;
        loan.ContactPerson = command.ContactPerson;
        loan.ContactEmail = command.ContactEmail;
        loan.Amount = command.Amount;
        loan.InterestRate = command.InterestRate;
        loan.TermYears = command.TermYears;
        loan.Purpose = command.Purpose;

        var updatedLoan = await _loanRepository.UpdateAsync(loan);

        return new LoanDto
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
    }
}

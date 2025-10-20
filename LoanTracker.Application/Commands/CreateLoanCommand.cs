using LoanTracker.Application.DTOs;
using LoanTracker.Application.Interfaces;
using LoanTracker.Domain.Entities;
using LoanTracker.Domain.Enums;
using LoanTracker.Domain.Interfaces;

namespace LoanTracker.Application.Commands;

public record CreateLoanCommand(
    string BorrowerName,
    int BorrowerTypeId,
    string ContactPerson,
    string ContactEmail,
    decimal Amount,
    decimal InterestRate,
    int TermYears,
    string Purpose
);

public class CreateLoanCommandHandler : ICommandHandler<CreateLoanCommand, LoanDto>
{
    private readonly ILoanRepository _loanRepository;

    public CreateLoanCommandHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<LoanDto> HandleAsync(CreateLoanCommand command)
    {
        var loan = new Loan
        {
            LoanId = Guid.NewGuid(),
            BorrowerName = command.BorrowerName,
            BorrowerTypeId = command.BorrowerTypeId,
            ContactPerson = command.ContactPerson,
            ContactEmail = command.ContactEmail,
            Amount = command.Amount,
            InterestRate = command.InterestRate,
            TermYears = command.TermYears,
            Purpose = command.Purpose,
            ApplicationDate = DateTime.UtcNow,
            Status = LoanStatus.Open,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdLoan = await _loanRepository.CreateAsync(loan);

        return new LoanDto
        {
            LoanId = createdLoan.LoanId,
            BorrowerName = createdLoan.BorrowerName,
            BorrowerTypeId = createdLoan.BorrowerTypeId,
            BorrowerTypeName = createdLoan.BorrowerType?.TypeName ?? string.Empty,
            ContactPerson = createdLoan.ContactPerson,
            ContactEmail = createdLoan.ContactEmail,
            Amount = createdLoan.Amount,
            InterestRate = createdLoan.InterestRate,
            TermYears = createdLoan.TermYears,
            Purpose = createdLoan.Purpose,
            ApplicationDate = createdLoan.ApplicationDate,
            Status = createdLoan.Status,
            DecisionDate = createdLoan.DecisionDate,
            ReviewerNotes = createdLoan.ReviewerNotes,
            CreatedAt = createdLoan.CreatedAt,
            UpdatedAt = createdLoan.UpdatedAt
        };
    }
}

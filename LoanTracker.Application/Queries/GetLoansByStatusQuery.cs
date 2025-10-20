using LoanTracker.Application.DTOs;
using LoanTracker.Application.Interfaces;
using LoanTracker.Domain.Enums;
using LoanTracker.Domain.Interfaces;

namespace LoanTracker.Application.Queries;

public record GetLoansByStatusQuery(LoanStatus Status);

public class GetLoansByStatusQueryHandler : IQueryHandler<GetLoansByStatusQuery, IEnumerable<LoanDto>>
{
    private readonly ILoanRepository _loanRepository;

    public GetLoansByStatusQueryHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<IEnumerable<LoanDto>> HandleAsync(GetLoansByStatusQuery query)
    {
        var loans = await _loanRepository.GetByStatusAsync(query.Status);

        return loans.Select(loan => new LoanDto
        {
            LoanId = loan.LoanId,
            BorrowerName = loan.BorrowerName,
            BorrowerTypeId = loan.BorrowerTypeId,
            BorrowerTypeName = loan.BorrowerType?.TypeName ?? string.Empty,
            ContactPerson = loan.ContactPerson,
            ContactEmail = loan.ContactEmail,
            Amount = loan.Amount,
            InterestRate = loan.InterestRate,
            TermYears = loan.TermYears,
            Purpose = loan.Purpose,
            ApplicationDate = loan.ApplicationDate,
            Status = loan.Status,
            DecisionDate = loan.DecisionDate,
            ReviewerNotes = loan.ReviewerNotes,
            CreatedAt = loan.CreatedAt,
            UpdatedAt = loan.UpdatedAt
        });
    }
}

using LoanTracker.Application.DTOs;
using LoanTracker.Application.Interfaces;
using LoanTracker.Domain.Interfaces;

namespace LoanTracker.Application.Queries;

public record GetAllLoansQuery();

public class GetAllLoansQueryHandler : IQueryHandler<GetAllLoansQuery, IEnumerable<LoanDto>>
{
    private readonly ILoanRepository _loanRepository;

    public GetAllLoansQueryHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<IEnumerable<LoanDto>> HandleAsync(GetAllLoansQuery query)
    {
        var loans = await _loanRepository.GetAllAsync();

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

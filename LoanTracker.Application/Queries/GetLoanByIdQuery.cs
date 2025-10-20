using LoanTracker.Application.DTOs;
using LoanTracker.Application.Interfaces;
using LoanTracker.Domain.Interfaces;

namespace LoanTracker.Application.Queries;

public record GetLoanByIdQuery(Guid LoanId);

public class GetLoanByIdQueryHandler : IQueryHandler<GetLoanByIdQuery, LoanDto?>
{
    private readonly ILoanRepository _loanRepository;

    public GetLoanByIdQueryHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<LoanDto?> HandleAsync(GetLoanByIdQuery query)
    {
        var loan = await _loanRepository.GetByIdAsync(query.LoanId);

        if (loan == null)
        {
            return null;
        }

        return new LoanDto
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
        };
    }
}

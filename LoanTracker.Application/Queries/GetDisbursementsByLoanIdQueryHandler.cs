using LoanTracker.Application.Interfaces;
using LoanTracker.Domain.Interfaces;

namespace LoanTracker.Application.Queries;

public class GetDisbursementsByLoanIdQueryHandler : IQueryHandler<GetDisbursementsByLoanIdQuery, IEnumerable<DisbursementDto>>
{
    private readonly IDisbursementRepository _disbursementRepository;

    public GetDisbursementsByLoanIdQueryHandler(IDisbursementRepository disbursementRepository)
    {
        _disbursementRepository = disbursementRepository;
    }

    public async Task<IEnumerable<DisbursementDto>> HandleAsync(GetDisbursementsByLoanIdQuery query)
    {
        var disbursements = await _disbursementRepository.GetByLoanIdAsync(query.LoanId);

        return disbursements.Select(d => new DisbursementDto
        {
            DisbursementId = d.DisbursementId,
            ProjectId = d.ProjectId,
            LoanId = query.LoanId,
            ProjectName = d.Project?.ProjectName ?? "Unknown Project",
            Amount = d.AmountValue,
            Currency = d.AmountCurrency,
            DisbursementDate = d.DisbursementDate,
            RecipientName = d.RecipientName,
            RecipientDetails = d.RecipientDetails,
            CreatedAt = d.CreatedAt,
            IsBackdated = d.DisbursementDate < d.CreatedAt.Date
        });
    }
}

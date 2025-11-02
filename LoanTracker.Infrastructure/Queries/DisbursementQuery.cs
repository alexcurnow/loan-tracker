using LoanTracker.Application.Interfaces;
using LoanTracker.Infrastructure.Projections;
using Marten;

namespace LoanTracker.Infrastructure.Queries;

public class DisbursementQuery : IDisbursementQuery
{
    private readonly IDocumentSession _session;

    public DisbursementQuery(IDocumentSession session)
    {
        _session = session;
    }

    public async Task<IEnumerable<DisbursementReadModel>> GetByLoanIdAsync(Guid loanId)
    {
        var projection = await _session.LoadAsync<DisbursementHistoryProjection>(loanId);

        if (projection == null || !projection.Disbursements.Any())
        {
            return Enumerable.Empty<DisbursementReadModel>();
        }

        return projection.Disbursements.Select(d => new DisbursementReadModel(
            d.DisbursementId,
            d.ProjectId,
            d.Amount,
            d.Currency,
            d.DisbursementDate,
            d.RecipientName,
            d.RecipientDetails,
            d.RecordedAt,
            d.IsBackdated
        ));
    }
}

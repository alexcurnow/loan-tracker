using LoanTracker.Domain.Events;
using Marten.Events.Aggregation;

namespace LoanTracker.Infrastructure.Projections;

/// <summary>
/// Marten projection that maintains disbursement history for each loan
/// Updates whenever DisbursementIssued event occurs
/// </summary>
public class DisbursementHistoryViewProjection : SingleStreamProjection<DisbursementHistoryProjection, Guid>
{
    public DisbursementHistoryViewProjection()
    {
        // Tell Marten the projection name
        ProjectionName = "DisbursementHistory";
    }

    public DisbursementHistoryProjection Create(DisbursementIssued @event)
    {
        return new DisbursementHistoryProjection
        {
            Id = @event.LoanId,
            Disbursements = new List<DisbursementRecord>
            {
                new DisbursementRecord
                {
                    DisbursementId = @event.DisbursementId,
                    ProjectId = @event.ProjectId,
                    Amount = @event.Amount.Amount,
                    Currency = @event.Amount.Currency,
                    DisbursementDate = @event.DisbursementDate,
                    RecipientName = @event.RecipientName,
                    RecipientDetails = @event.RecipientDetails,
                    RecordedAt = @event.OccurredAt
                }
            },
            TotalDisbursed = @event.Amount.Amount,
            LastDisbursementDate = @event.DisbursementDate,
            LastUpdated = DateTime.UtcNow
        };
    }

    public void Apply(DisbursementIssued @event, DisbursementHistoryProjection projection)
    {
        projection.Disbursements.Add(new DisbursementRecord
        {
            DisbursementId = @event.DisbursementId,
            ProjectId = @event.ProjectId,
            Amount = @event.Amount.Amount,
            Currency = @event.Amount.Currency,
            DisbursementDate = @event.DisbursementDate,
            RecipientName = @event.RecipientName,
            RecipientDetails = @event.RecipientDetails,
            RecordedAt = @event.OccurredAt
        });

        projection.TotalDisbursed = projection.Disbursements.Sum(d => d.Amount);
        projection.LastDisbursementDate = projection.Disbursements.Max(d => d.DisbursementDate);
        projection.LastUpdated = DateTime.UtcNow;
    }
}

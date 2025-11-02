using LoanTracker.Domain.Events;

namespace LoanTracker.Infrastructure.Projections;

/// <summary>
/// Projection storing all disbursements for a loan
/// Updated whenever DisbursementIssued event occurs
/// </summary>
public class DisbursementHistoryProjection
{
    public Guid Id { get; set; }  // LoanId
    public List<DisbursementRecord> Disbursements { get; set; } = new();
    public decimal TotalDisbursed { get; set; }
    public DateTime? LastDisbursementDate { get; set; }
    public DateTime LastUpdated { get; set; }

    // Marten requires static Create method for first event
    public static DisbursementHistoryProjection Create(DisbursementIssued @event)
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

    // Marten requires static Apply method for subsequent events
    public static DisbursementHistoryProjection Apply(DisbursementIssued @event, DisbursementHistoryProjection projection)
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

        return projection;
    }
}

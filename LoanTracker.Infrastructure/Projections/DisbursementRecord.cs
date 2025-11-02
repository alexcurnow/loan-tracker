namespace LoanTracker.Infrastructure.Projections;

/// <summary>
/// Read model for a single disbursement
/// Stored as part of DisbursementHistoryProjection
/// </summary>
public class DisbursementRecord
{
    public Guid DisbursementId { get; set; }
    public Guid ProjectId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime DisbursementDate { get; set; }  // When funds were released
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientDetails { get; set; } = string.Empty;
    public DateTime RecordedAt { get; set; }  // When event occurred
    public bool IsBackdated => DisbursementDate < RecordedAt.Date;
}

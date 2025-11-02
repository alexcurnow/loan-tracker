namespace LoanTracker.Infrastructure.Projections;

/// <summary>
/// Projection storing pre-calculated interest accruals for a loan
/// Recalculated whenever DisbursementIssued event occurs
/// This is the performance optimization - calculate once, query many times
/// </summary>
public class InterestAccrualHistoryProjection
{
    public Guid Id { get; set; }  // LoanId
    public List<AccrualRecord> Accruals { get; set; } = new();
    public decimal TotalInterestAccrued { get; set; }
    public decimal CurrentPrincipalBalance { get; set; }
    public DateTime LastCalculated { get; set; }
}

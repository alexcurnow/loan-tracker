namespace LoanTracker.Domain.Entities;

/// <summary>
/// Read model for interest accruals - maintained by Marten projections
/// This is a snapshot/cache of calculated accruals, rebuilt from events
/// Demonstrates event sourcing projection pattern
/// </summary>
public class InterestAccrualSnapshot
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid LoanId { get; set; }
    public DateTime AccrualDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PrincipalBalance { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal InterestRate { get; set; }
    public string Period { get; set; } = string.Empty;
    public string Currency { get; set; } = "USD";
    public DateTime LastUpdated { get; set; }
    public int Version { get; set; } // For optimistic concurrency

    // Navigation
    public Loan? Loan { get; set; }
}

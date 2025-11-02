namespace LoanTracker.Infrastructure.Projections;

/// <summary>
/// Read model for a single monthly interest accrual
/// </summary>
public class AccrualRecord
{
    public DateTime AccrualDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PrincipalBalance { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal InterestRate { get; set; }
    public string Period { get; set; } = string.Empty;  // e.g., "January 2024"
    public string Currency { get; set; } = "USD";
}

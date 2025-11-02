using LoanTracker.Domain.ValueObjects;

namespace LoanTracker.Domain.Entities;

/// <summary>
/// Represents a project funded by a loan
/// Typically 2 projects per loan
/// Can exist before loan approval
/// </summary>
public class Project
{
    public Guid ProjectId { get; set; }
    public Guid LoanId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public decimal BudgetAmount { get; set; }  // Stored as decimal for EF Core
    public string BudgetCurrency { get; set; } = "USD";  // Stored separately for EF Core
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Loan? Loan { get; set; }
    // Disbursements are now event-sourced via Marten, not stored as navigation property

    // Domain property using Value Object
    public Money Budget
    {
        get => Money.FromDecimal(BudgetAmount, BudgetCurrency);
        set
        {
            BudgetAmount = value.Amount;
            BudgetCurrency = value.Currency;
        }
    }
}

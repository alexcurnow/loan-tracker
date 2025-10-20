using LoanTracker.Domain.Enums;

namespace LoanTracker.Domain.Entities;

public class Loan
{
    public Guid LoanId { get; set; }
    public string BorrowerName { get; set; } = string.Empty;
    public int BorrowerTypeId { get; set; }
    public string ContactPerson { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
    public int TermYears { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
    public LoanStatus Status { get; set; }
    public DateTime? DecisionDate { get; set; }
    public string? ReviewerNotes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation property
    public BorrowerType BorrowerType { get; set; } = null!;
}

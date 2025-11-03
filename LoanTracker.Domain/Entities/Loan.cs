using LoanTracker.Domain.Enums;
using LoanTracker.Domain.Types;
using LoanTracker.Domain.ValueObjects;

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
    public TermYears TermYears { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
    public LoanStatus Status { get; set; }
    public int DueDay { get; set; } = 20;  // Day of month payments/accruals due (default 20th)
    public DateTime? DecisionDate { get; set; }
    public string? ReviewerNotes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public BorrowerType BorrowerType { get; set; } = null!;
    public ICollection<Project> Projects { get; set; } = new List<Project>();

    // Domain properties using Value Objects (computed, not stored)
    public Money LoanAmount => Money.Dollars(Amount);
    public ValueObjects.InterestRate Rate => ValueObjects.InterestRate.FromPercentage(InterestRate);

    // Helper property: Date interest accruals generate (15 days before due day)
    public int AccrualGenerationDay => DueDay - 15 > 0 ? DueDay - 15 : DueDay - 15 + 30;
}

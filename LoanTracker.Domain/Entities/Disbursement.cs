using LoanTracker.Domain.ValueObjects;

namespace LoanTracker.Domain.Entities;

/// <summary>
/// Represents a disbursement of loan funds to a project
/// IMMUTABLE once created - if wrong, must create offsetting entry
/// Can be backdated (DisbursementDate in the past)
/// </summary>
public class Disbursement
{
    public Guid DisbursementId { get; init; }
    public Guid ProjectId { get; init; }
    public decimal AmountValue { get; init; }  // Stored as decimal for EF Core
    public string AmountCurrency { get; init; } = "USD";  // Stored separately for EF Core
    public DateTime DisbursementDate { get; init; }  // When funds were actually released (can be backdated)
    public string RecipientName { get; init; } = string.Empty;
    public string RecipientDetails { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }  // When record was created in system

    // Navigation property
    public Project? Project { get; init; }

    // Domain property using Value Object
    public Money Amount
    {
        get => Money.FromDecimal(AmountValue, AmountCurrency);
        init
        {
            AmountValue = value.Amount;
            AmountCurrency = value.Currency;
        }
    }

    public Disbursement()
    {
        // Default constructor for EF Core
    }

    public static Disbursement Create(
        Guid projectId,
        Money amount,
        DateTime disbursementDate,
        string recipientName,
        string recipientDetails)
    {
        return new Disbursement
        {
            DisbursementId = Guid.NewGuid(),
            ProjectId = projectId,
            Amount = amount,
            DisbursementDate = disbursementDate,
            RecipientName = recipientName,
            RecipientDetails = recipientDetails,
            CreatedAt = DateTime.UtcNow
        };
    }
}

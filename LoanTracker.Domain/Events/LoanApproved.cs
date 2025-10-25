using LoanTracker.Domain.ValueObjects;

namespace LoanTracker.Domain.Events;

/// <summary>
/// Event raised when a loan is approved
/// This triggers workflow transitions and sets up for disbursements
/// </summary>
public sealed record LoanApproved : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;

    public Guid LoanId { get; init; }
    public string BorrowerName { get; init; } = string.Empty;
    public Money LoanAmount { get; init; } = Money.Zero;
    public decimal InterestRatePercentage { get; init; }
    public int TermYears { get; init; }
    public int DueDay { get; init; }
    public string ApprovedBy { get; init; } = string.Empty;
    public string ReviewerNotes { get; init; } = string.Empty;
}

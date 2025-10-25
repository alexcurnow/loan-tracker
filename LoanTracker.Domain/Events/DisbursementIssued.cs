using LoanTracker.Domain.ValueObjects;

namespace LoanTracker.Domain.Events;

/// <summary>
/// Event raised when loan funds are disbursed to a project
/// CRITICAL: Can be backdated! DisbursementDate may be in the past.
/// This triggers:
/// - Loan status transition (Approved → Construction on first disbursement)
/// - Interest accrual recalculation (temporal event ordering)
/// </summary>
public sealed record DisbursementIssued : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;  // When event was recorded

    public Guid DisbursementId { get; init; }
    public Guid LoanId { get; init; }
    public Guid ProjectId { get; init; }
    public Money Amount { get; init; } = Money.Zero;
    public DateTime DisbursementDate { get; init; }  // When funds were actually released (can be backdated)
    public string RecipientName { get; init; } = string.Empty;
    public string RecipientDetails { get; init; } = string.Empty;
    public string IssuedBy { get; init; } = string.Empty;

    // Helper to identify backdated events
    public bool IsBackdated => DisbursementDate < OccurredAt.Date;
}

using LoanTracker.Domain.Enums;

namespace LoanTracker.Domain.Events;

/// <summary>
/// Event raised when a loan's status changes
/// Tracks workflow transitions (e.g., Open → AwaitingReview → Approved → Construction → Active)
/// </summary>
public sealed record LoanStatusChanged : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;

    public Guid LoanId { get; init; }
    public LoanStatus FromStatus { get; init; }
    public LoanStatus ToStatus { get; init; }
    public string Reason { get; init; } = string.Empty;
    public string ChangedBy { get; init; } = string.Empty;
}

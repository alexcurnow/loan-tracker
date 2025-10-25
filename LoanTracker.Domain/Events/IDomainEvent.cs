namespace LoanTracker.Domain.Events;

/// <summary>
/// Base interface for all domain events
/// Events represent something that has happened in the domain
/// </summary>
public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredAt { get; }
}

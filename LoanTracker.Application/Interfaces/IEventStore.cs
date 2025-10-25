using LoanTracker.Domain.Events;

namespace LoanTracker.Application.Interfaces;

/// <summary>
/// Interface for event store operations
/// Abstracts the underlying event sourcing implementation (Marten)
/// </summary>
public interface IEventStore
{
    /// <summary>
    /// Append one or more events to a stream
    /// </summary>
    /// <param name="streamId">The aggregate ID (e.g., LoanId)</param>
    /// <param name="events">Events to append</param>
    Task AppendToStreamAsync(Guid streamId, params IDomainEvent[] events);

    /// <summary>
    /// Append events and immediately save changes
    /// </summary>
    Task AppendToStreamAndSaveAsync(Guid streamId, params IDomainEvent[] events);

    /// <summary>
    /// Get all events for a specific stream
    /// </summary>
    Task<IEnumerable<IDomainEvent>> GetStreamEventsAsync(Guid streamId);

    /// <summary>
    /// Save any pending changes
    /// </summary>
    Task SaveChangesAsync();
}

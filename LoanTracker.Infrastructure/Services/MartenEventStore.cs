using LoanTracker.Application.Interfaces;
using LoanTracker.Domain.Events;
using Marten;

namespace LoanTracker.Infrastructure.Services;

/// <summary>
/// Marten-based implementation of the event store
/// Provides event sourcing capabilities using PostgreSQL as the backing store
/// </summary>
public class MartenEventStore : IEventStore
{
    private readonly IDocumentSession _session;

    public MartenEventStore(IDocumentSession session)
    {
        _session = session;
    }

    public async Task AppendToStreamAsync(Guid streamId, params IDomainEvent[] events)
    {
        if (events == null || events.Length == 0)
            return;

        _session.Events.Append(streamId, events);
    }

    public async Task AppendToStreamAndSaveAsync(Guid streamId, params IDomainEvent[] events)
    {
        await AppendToStreamAsync(streamId, events);
        await SaveChangesAsync();
    }

    public async Task<IEnumerable<IDomainEvent>> GetStreamEventsAsync(Guid streamId)
    {
        var events = await _session.Events.FetchStreamAsync(streamId);
        return events.Select(e => e.Data).Cast<IDomainEvent>();
    }

    public async Task SaveChangesAsync()
    {
        await _session.SaveChangesAsync();
    }
}

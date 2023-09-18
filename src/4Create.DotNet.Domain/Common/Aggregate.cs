using System.Text.Json.Serialization;

namespace _4Create.DotNet.Domain.Common;


public abstract class Aggregate<TId> where TId : struct
{
    [JsonIgnore]
    public IReadOnlyList<IDomainEvent> PendingEvents => _pendingEvents.AsReadOnly();
        
    private readonly List<IDomainEvent> _pendingEvents;
    
    public TId Id { get; private set; }

    public Aggregate(TId id) 
    {
        _pendingEvents = new List<IDomainEvent>();
        Id = id;
    }

    protected void AddEvent<TEvent>(TEvent e) where TEvent : IDomainEvent
    {
        _pendingEvents.Add(e);
    }

    public void ClearAllEvents()
    {
        _pendingEvents.Clear();
    }

    public IReadOnlyList<IDomainEvent> PopAllEvents()
    {
        var events = _pendingEvents.ToList();
        ClearAllEvents();

        return events;
    }
}

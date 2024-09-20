using TodoApp.Domain.Events;

namespace TodoApp.Infrastructure.EventStore
{
    public class InMemoryEventStore
    {
        private readonly Dictionary<Guid, List<ITodoEvent>> _eventStreams = new Dictionary<Guid, List<ITodoEvent>>();

        public Task<List<ITodoEvent>> GetEventsAsync(Guid aggregateId)
        {
            if (!_eventStreams.ContainsKey(aggregateId))
                return Task.FromResult(new List<ITodoEvent>());

            return Task.FromResult(_eventStreams[aggregateId]);
        }

        public Task<List<List<ITodoEvent>>> GetEventsAsync()
        {
            var allEventStreams = _eventStreams.Values.ToList();
            return Task.FromResult(allEventStreams);
        }

        public Task SaveEventsAsync(Guid aggregateId, IReadOnlyCollection<ITodoEvent> events)
        {
            if (!_eventStreams.ContainsKey(aggregateId))
                _eventStreams[aggregateId] = new List<ITodoEvent>();

            _eventStreams[aggregateId].AddRange(events);
            return Task.CompletedTask;
        }
    }
}

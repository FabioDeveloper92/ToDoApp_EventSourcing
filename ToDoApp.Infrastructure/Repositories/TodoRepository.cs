using TodoApp.Domain.Aggregates;
using TodoApp.Infrastructure.EventStore;

namespace TodoApp.Infrastructure.Repositories
{
    public interface ITodoRepository
    {
        Task<TodoAggregate> GetByIdAsync(Guid id);
        Task<TodoAggregate[]> GetAllTodosAsync();
        Task SaveAsync(TodoAggregate aggregate);
    }

    public class TodoRepository : ITodoRepository
    {
        private readonly InMemoryEventStore _eventStore;

        public TodoRepository(InMemoryEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<TodoAggregate> GetByIdAsync(Guid id)
        {
            var events = await _eventStore.GetEventsAsync(id);
            if (events == null || !events.Any())
                throw new Exception("No events found for the given ID");

            var aggregate = new TodoAggregate();
            foreach (var @event in events)
                aggregate.Apply(@event);

            return aggregate;
        }

        public async Task SaveAsync(TodoAggregate aggregate)
        {
            var events = aggregate.GetUncommittedChanges();
            if (!events.Any())
                return;

            await _eventStore.SaveEventsAsync(aggregate.Id, events);

            aggregate.ClearUncommittedChanges();
        }

        public async Task<TodoAggregate[]> GetAllTodosAsync()
        {
            // Ottieni tutti gli stream di eventi
            var allEventStreams = await _eventStore.GetEventsAsync();
            var aggregates = new List<TodoAggregate>();

            // Ricostruisci ogni aggregato dagli eventi
            foreach (var eventStream in allEventStreams)
            {
                var aggregate = new TodoAggregate();
                foreach (var @event in eventStream)
                {
                    aggregate.Apply(@event);
                }
                aggregates.Add(aggregate);
            }

            return aggregates.ToArray();
        }
    }
}

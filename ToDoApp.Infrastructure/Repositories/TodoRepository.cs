using TodoApp.Domain.Aggregates;
using TodoApp.Infrastructure.EventStore;

namespace TodoApp.Infrastructure.Repositories
{
    public interface ITodoRepository
    {
        Task<TodoAggregate> GetByIdAsync(Guid id);
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
    }
}

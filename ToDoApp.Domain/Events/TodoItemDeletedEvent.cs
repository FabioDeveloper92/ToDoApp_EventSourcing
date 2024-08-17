namespace TodoApp.Domain.Events
{
    public class TodoItemDeletedEvent : ITodoEvent
    {
        public Guid Id { get; }

        public TodoItemDeletedEvent(Guid id)
        {
            Id = id;
        }
    }
}

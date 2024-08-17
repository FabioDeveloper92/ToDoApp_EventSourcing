namespace TodoApp.Domain.Events
{
    public class TodoItemUpdatedEvent : ITodoEvent
    {
        public Guid Id { get; }
        public string Title { get; }
        public string Description { get; }

        public TodoItemUpdatedEvent(Guid id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }
    }

}

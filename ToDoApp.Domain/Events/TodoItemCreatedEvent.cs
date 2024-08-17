namespace TodoApp.Domain.Events
{
    public class TodoItemCreatedEvent : ITodoEvent
    {
        public Guid Id { get; }
        public string Title { get; }
        public string Description { get; }

        public TodoItemCreatedEvent(Guid id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }
    }

}

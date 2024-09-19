using TodoApp.Domain.Enum;

namespace TodoApp.Domain.Events
{
    public class TodoItemCreatedEvent(Guid id, string title, string description, TodoStatusEnum status, DateTime? expiredDate) : ITodoEvent
    {
        public Guid Id { get; } = id;
        public string Title { get; } = title;
        public string Description { get; } = description;
        public TodoStatusEnum Status { get; } = status;
        public DateTime? ExpiredDate { get; } = expiredDate;
    }

}

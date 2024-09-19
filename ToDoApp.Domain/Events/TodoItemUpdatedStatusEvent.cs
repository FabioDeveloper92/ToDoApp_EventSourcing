using TodoApp.Domain.Enum;

namespace TodoApp.Domain.Events
{
    public class TodoItemUpdatedStatusEvent(Guid id, TodoStatusEnum status) : ITodoEvent
    {
        public Guid Id { get; } = id;
        public TodoStatusEnum Status { get; } = status;
    }

}

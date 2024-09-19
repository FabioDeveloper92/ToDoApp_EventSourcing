using TodoApp.Domain.Enum;

namespace TodoApp.Application.Commands
{
    public class UpdateTodoCommand(Guid id, string title, string description, TodoStatusEnum status, DateTime? expiredDate)
    {
        public Guid Id { get; private set; } = id;
        public string Title { get; private set; } = title;
        public string Description { get; private set; } = description;
        public TodoStatusEnum Status { get; private set; } = status;
        public DateTime? ExpiredDate { get; private set; } = expiredDate;
    }

}

using TodoApp.Domain.Enum;

namespace TodoApp.Application.Commands
{
    public class CreateTodoCommand(Guid id, string title, string description, TodoStatusEnum status, DateTime? expiredDate)
    {
        public Guid Id { get; set; } = id;
        public string Title { get; set; } = title;
        public string Description { get; set; } = description;
        public TodoStatusEnum Status { get; set; } = status;
        public DateTime? ExpiredDate { get; set; } = expiredDate;
    }

}

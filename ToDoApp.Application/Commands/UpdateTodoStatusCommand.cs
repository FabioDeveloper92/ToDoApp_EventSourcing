using TodoApp.Domain.Enum;

namespace TodoApp.Application.Commands
{
    public class UpdateTodoStatusCommand(Guid id, TodoStatusEnum status)
    {
        public Guid Id { get; private set; } = id;
        public TodoStatusEnum Status { get; private set; } = status;
    }

}

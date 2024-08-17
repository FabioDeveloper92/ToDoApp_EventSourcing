namespace TodoApp.Application.Commands
{
    public class DeleteTodoCommand
    {
        public Guid Id { get; set; }

        public DeleteTodoCommand(Guid id)
        {
            Id = id;
        }
    }

}

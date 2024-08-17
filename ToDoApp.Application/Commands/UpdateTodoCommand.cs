namespace TodoApp.Application.Commands
{
    public class UpdateTodoCommand
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        public UpdateTodoCommand(Guid id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }
    }

}

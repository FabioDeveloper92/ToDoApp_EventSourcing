namespace TodoApp.Application.Commands
{
    public class CreateTodoCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public CreateTodoCommand(Guid id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }
    }

}

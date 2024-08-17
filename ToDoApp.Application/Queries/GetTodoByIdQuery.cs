namespace TodoApp.Application.Queries
{
    public class GetTodoByIdQuery
    {
        public Guid Id { get; set; }

        public GetTodoByIdQuery(Guid id)
        {
            Id = id;
        }
    }

}

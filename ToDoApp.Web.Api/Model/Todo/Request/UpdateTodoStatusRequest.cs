namespace TodoApp.Web.Api.Model.Todo.Request
{
    public class UpdateTodoStatusRequest
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }
}

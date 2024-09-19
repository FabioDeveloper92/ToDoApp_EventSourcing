using TodoApp.Domain.Enum;

namespace TodoApp.Web.Api.Model.Todo.Request
{
    public class CreateTodoRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
}

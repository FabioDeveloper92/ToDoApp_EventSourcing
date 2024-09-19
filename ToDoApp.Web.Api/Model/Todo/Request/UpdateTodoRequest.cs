using TodoApp.Domain.Enum;

namespace TodoApp.Web.Api.Model.Todo.Request
{
    public class UpdateTodoRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime? ExpiredDate { get; set; }
    }
}

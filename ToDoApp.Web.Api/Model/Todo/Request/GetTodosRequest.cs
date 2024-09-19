namespace TodoApp.Web.Api.Model.Todo.Request
{
    public class GetTodosRequest
    {
        public string? Title { get; set; }
        public int? Status { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}

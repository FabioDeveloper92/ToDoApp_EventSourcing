using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Commands;
using TodoApp.Application.Services.TodoApp.Application.Services;
using TodoApp.Web.Api.Model.Todo.Request;

namespace TodoApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTodo([FromBody] CreateTodoRequest request)
        {
            var command = new CreateTodoCommand(Guid.NewGuid(), request.Title, request.Description);

            var result = await _todoService.CreateTodoAsync(command);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateTodo([FromBody] UpdateTodoRequest request)
        {
            var command = new UpdateTodoCommand(request.Id, request.Title, request.Description);

            await _todoService.UpdateTodoAsync(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(Guid id)
        {
            await _todoService.DeleteTodoAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoById(Guid id)
        {
            var result = await _todoService.GetTodoByIdAsync(id);
            return Ok(result);
        }
    }
}
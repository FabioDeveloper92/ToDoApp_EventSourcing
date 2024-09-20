using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.Repositories;

namespace TodoApp.Application.Queries
{
    public interface ITodoQueryHandlers
    {
        Task<TodoItem> Handle(GetTodoByIdQuery query);
        Task<TodoItem[]> Handle(GetTodosQuery query);
    }

    public class TodoQueryHandlers : ITodoQueryHandlers
    {
        private readonly ITodoRepository _todoRepository;

        public TodoQueryHandlers(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<TodoItem> Handle(GetTodoByIdQuery query)
        {
            var todoAggregate = await _todoRepository.GetByIdAsync(query.Id);
            return todoAggregate.GetTodoItemById(query.Id);
        }

        public async Task<TodoItem[]> Handle(GetTodosQuery query)
        {
            var aggregates = await _todoRepository.GetAllTodosAsync();

            return aggregates
                .SelectMany(a => a.GetTodoItems())
                .ToArray();
        }
    }
}

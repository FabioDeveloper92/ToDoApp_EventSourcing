using TodoApp.Application.Commands;
using TodoApp.Domain.Aggregates;
using TodoApp.Domain.Entities;
using TodoApp.Infrastructure.Repositories;

namespace TodoApp.Application.Services
{
    namespace TodoApp.Application.Services
    {
        public interface ITodoService
        {
            Task<Guid> CreateTodoAsync(CreateTodoCommand command);
            Task UpdateTodoAsync(UpdateTodoCommand command);
            Task DeleteTodoAsync(Guid id);
            Task<TodoItem> GetTodoByIdAsync(Guid id);
        }

        public class TodoService : ITodoService
        {
            private readonly ITodoRepository _todoRepository;

            public TodoService(ITodoRepository todoRepository)
            {
                _todoRepository = todoRepository;
            }

            public async Task<Guid> CreateTodoAsync(CreateTodoCommand command)
            {
                var aggregate = new TodoAggregate();
                aggregate.CreateTodoItem(command.Id, command.Title, command.Description);
                await _todoRepository.SaveAsync(aggregate);
                return command.Id;
            }

            public async Task UpdateTodoAsync(UpdateTodoCommand command)
            {
                var aggregate = await _todoRepository.GetByIdAsync(command.Id);
                aggregate.UpdateTodoItem(command.Id, command.Title, command.Description);
                await _todoRepository.SaveAsync(aggregate);
            }

            public async Task DeleteTodoAsync(Guid id)
            {
                var aggregate = await _todoRepository.GetByIdAsync(id);
                aggregate.DeleteTodoItem(id);
                await _todoRepository.SaveAsync(aggregate);
            }

            public async Task<TodoItem> GetTodoByIdAsync(Guid id)
            {
                var aggregate = await _todoRepository.GetByIdAsync(id);
                return aggregate.GetTodoItemById(id);
            }
        }
    }

}

using TodoApp.Domain.Aggregates;
using TodoApp.Infrastructure.Repositories;

namespace TodoApp.Application.Commands
{
    public interface ITodoCommandHandlers
    {
        Task Handle(CreateTodoCommand command);
        Task Handle(UpdateTodoCommand command);
        Task Handle(DeleteTodoCommand command);
    }

    public class TodoCommandHandlers: ITodoCommandHandlers
    {
        private readonly ITodoRepository _todoRepository;

        public TodoCommandHandlers(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task Handle(CreateTodoCommand command)
        {
            var todoAggregate = new TodoAggregate();
            todoAggregate.CreateTodoItem(command.Id, command.Title, command.Description);
            await _todoRepository.SaveAsync(todoAggregate);
        }
        public async Task Handle(UpdateTodoCommand command)
        {
            var todoAggregate = await _todoRepository.GetByIdAsync(command.Id);
            todoAggregate.UpdateTodoItem(command.Id, command.Title, command.Description);
            await _todoRepository.SaveAsync(todoAggregate);
        }
        public async Task Handle(DeleteTodoCommand command)
        {
            var todoAggregate = await _todoRepository.GetByIdAsync(command.Id);
            todoAggregate.DeleteTodoItem(command.Id);
            await _todoRepository.SaveAsync(todoAggregate);
        }

    }
}

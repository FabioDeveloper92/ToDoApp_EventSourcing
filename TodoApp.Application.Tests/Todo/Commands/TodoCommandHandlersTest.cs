using Moq;
using TodoApp.Application.Commands;
using TodoApp.Domain.Aggregates;
using TodoApp.Infrastructure.Repositories;
using Xunit;

namespace TodoApp.Application.Tests.Todo.Commands
{
    public class TodoCommandHandlersTest
    {
        private readonly Mock<ITodoRepository> _mockTodoRepository;
        private readonly TodoCommandHandlers _handler;

        public TodoCommandHandlersTest()
        {
            _mockTodoRepository = new Mock<ITodoRepository>();
            _handler = new TodoCommandHandlers(_mockTodoRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateTodoItem()
        {
            // Arrange
            var command = new CreateTodoCommand(Guid.NewGuid(), "Test Todo", "Test Description");

            // Act
            await _handler.Handle(command);

            // Assert
            _mockTodoRepository.Verify(x => x.SaveAsync(It.IsAny<TodoAggregate>()), Times.Once);
        }
    }
}

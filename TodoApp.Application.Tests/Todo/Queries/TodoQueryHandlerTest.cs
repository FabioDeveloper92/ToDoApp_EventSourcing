using FluentAssertions;
using Moq;
using TodoApp.Application.Queries;
using TodoApp.Domain.Aggregates;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Events;
using TodoApp.Infrastructure.Repositories;
using Xunit;

namespace TodoApp.Application.Tests
{
    public class TodoQueryHandlerTest
    {
        private readonly Mock<ITodoRepository> _mockTodoRepository;
        private readonly TodoQueryHandlers _handler;

        public TodoQueryHandlerTest()
        {
            _mockTodoRepository = new Mock<ITodoRepository>();
            _handler = new TodoQueryHandlers(_mockTodoRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnTodoItem()
        {
            // Arrange
            var todoId = Guid.NewGuid(); 
            var todoName = "My Test Update";
            var todoDescription = "My Test Desc";
            var todoStatus = Domain.Enum.TodoStatusEnum.Draft;
            var todoExpiredDate = new DateTime();

            var todoItem = new TodoItem(todoId, todoName, todoDescription, todoStatus, todoExpiredDate);

            var todoAggregate = new TodoAggregate();
            todoAggregate.Apply(new TodoItemCreatedEvent(todoId, todoName, todoDescription, todoStatus, todoExpiredDate));

            _mockTodoRepository.Setup(x => x.GetByIdAsync(todoId)).ReturnsAsync(todoAggregate);

            // Act
            var result = await _handler.Handle(new GetTodoByIdQuery(todoId));

            // Assert
            result.Should().BeEquivalentTo(todoItem);
        }

        [Fact]
        public async Task Handle_ShouldReturnTodoItemUpdated()
        {
            // Arrange
            var todoId = Guid.NewGuid();
            var todoName = "My Test Update";
            var todoDescription = "My Test Desc";
            var todoStatus = Domain.Enum.TodoStatusEnum.Draft;
            var todoExpiredDate = new DateTime();

            var todoItem = new TodoItem(todoId, todoName, todoDescription, todoStatus, todoExpiredDate);

            var todoAggregate = new TodoAggregate();
            todoAggregate.Apply(new TodoItemCreatedEvent(todoId, "Test Todo", "Test Description", todoStatus, todoExpiredDate));
            todoAggregate.Apply(new TodoItemCreatedEvent(Guid.NewGuid(), "Test Todo2", "Test Description", todoStatus, todoExpiredDate));
            todoAggregate.Apply(new TodoItemUpdatedEvent(todoId, todoName, todoDescription, todoStatus, todoExpiredDate));

            _mockTodoRepository.Setup(x => x.GetByIdAsync(todoId)).ReturnsAsync(todoAggregate);

            // Act
            var result = await _handler.Handle(new GetTodoByIdQuery(todoId));

            // Assert
            result.Should().BeEquivalentTo(todoItem);
        }
    }
}

using FluentAssertions;
using Moq;
using TodoApp.Application.Queries;
using TodoApp.Domain.Aggregates;
using TodoApp.Domain.Entities;
using TodoApp.Domain.Enum;
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

        [Fact]
        public async Task Handle_ShouldReturnTodoItemStatusUpdated()
        {
            // Arrange
            var todoId = Guid.NewGuid();
            var todoName = "My Test Update";
            var todoDescription = "My Test Desc";
            var todoStatus = TodoStatusEnum.Completed;
            var todoExpiredDate = new DateTime();

            var todoItem = new TodoItem(todoId, todoName, todoDescription, todoStatus, todoExpiredDate);

            var todoAggregate = new TodoAggregate();
            todoAggregate.Apply(new TodoItemCreatedEvent(todoId, todoName, todoDescription, TodoStatusEnum.Draft, todoExpiredDate));
            todoAggregate.Apply(new TodoItemUpdatedStatusEvent(todoId, TodoStatusEnum.Working));
            todoAggregate.Apply(new TodoItemUpdatedStatusEvent(todoId, TodoStatusEnum.Completed));
            todoAggregate.Apply(new TodoItemUpdatedStatusEvent(todoId, todoStatus));

            _mockTodoRepository.Setup(x => x.GetByIdAsync(todoId)).ReturnsAsync(todoAggregate);

            // Act
            var result = await _handler.Handle(new GetTodoByIdQuery(todoId));

            // Assert
            result.Should().BeEquivalentTo(todoItem);
        }

        [Fact]
        public async Task Handle_ShouldReturnTodoItemIsNotDeleted()
        {
            // Arrange
            var todoId = Guid.NewGuid();
            var todoName = "My Test Update";
            var todoDescription = "My Test Desc";
            var todoStatus = TodoStatusEnum.Completed;
            var todoExpiredDate = new DateTime();

            var todoId2 = Guid.NewGuid();

            var todoItem = new TodoItem(todoId, todoName, todoDescription, todoStatus, todoExpiredDate);
            var todoItem2 = new TodoItem(todoId, todoName, todoDescription, todoStatus, todoExpiredDate);

            var todoAggregate = new TodoAggregate();
            var todoAggregate2 = new TodoAggregate();
            var todoAggregate3 = new TodoAggregate();

            todoAggregate.Apply(new TodoItemCreatedEvent(todoId, todoName, todoDescription, TodoStatusEnum.Draft, todoExpiredDate));

            todoAggregate2.Apply(new TodoItemCreatedEvent(Guid.NewGuid(), todoName, todoDescription, TodoStatusEnum.Draft, todoExpiredDate));

            todoAggregate3.Apply(new TodoItemCreatedEvent(todoId2, todoName, todoDescription, TodoStatusEnum.Draft, todoExpiredDate));

            todoAggregate.Apply(new TodoItemUpdatedStatusEvent(todoId, TodoStatusEnum.Working));

            todoAggregate3.Apply(new TodoItemUpdatedStatusEvent(todoId2, TodoStatusEnum.Deleted));

            todoAggregate.Apply(new TodoItemUpdatedStatusEvent(todoId, todoStatus));

            _mockTodoRepository
                .Setup(x => x.GetAllTodosAsync())
                .ReturnsAsync(new[] { todoAggregate, todoAggregate2, todoAggregate3 });

            // Act
            var result = await _handler.Handle(new GetTodosQuery());

            // Assert
            result.Should().HaveCount(2);
        }
    }
}

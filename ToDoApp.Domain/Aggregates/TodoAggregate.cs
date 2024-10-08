﻿using TodoApp.Domain.Entities;
using TodoApp.Domain.Enum;
using TodoApp.Domain.Events;

namespace TodoApp.Domain.Aggregates
{
    public class TodoAggregate
    {
        private readonly List<ITodoEvent> _changes = new List<ITodoEvent>();
        private readonly List<TodoItem> _todoItems = new List<TodoItem>();

        // L'ID dell'aggregato
        public Guid Id { get; private set; }

        // Costruttore per la creazione di un nuovo aggregato
        public TodoAggregate(Guid id)
        {
            Id = id;
        }

        // Costruttore per il caricamento di un aggregato esistente da eventi
        public TodoAggregate() { }

        public IReadOnlyList<ITodoEvent> GetUncommittedChanges() => _changes.AsReadOnly();

        public void CreateTodoItem(Guid id, string title, string description, TodoStatusEnum status, DateTime? expiredDate)
        {
            if (_todoItems.Any(t => t.Id == id))
                throw new Exception("Todo item already exists.");

            var todoItem = new TodoItem(id, title, description, status, expiredDate);
            _todoItems.Add(todoItem);
            _changes.Add(new TodoItemCreatedEvent(id, title, description, status, expiredDate));
        }

        public void UpdateTodoItem(Guid id, string title, string description, TodoStatusEnum status, DateTime? expiredDate)
        {
            var todoItem = _todoItems.SingleOrDefault(t => t.Id == id);
            if (todoItem == null || todoItem.Status == TodoStatusEnum.Deleted)
                throw new Exception("Todo item not found or has been deleted.");

            todoItem.Update(title, description, status, expiredDate);
            _changes.Add(new TodoItemUpdatedEvent(id, title, description, status, expiredDate));
        }

        public void UpdateTodoStatus(Guid id, TodoStatusEnum status)
        {
            var todoItem = _todoItems.Single(t => t.Id == id);
            todoItem.UpdateStatus(status);

            _changes.Add(new TodoItemUpdatedStatusEvent(id, status));
        }

        public void DeleteTodoItem(Guid id)
        {
            var todoItem = _todoItems.SingleOrDefault(t => t.Id == id);
            if (todoItem == null || todoItem.Status == TodoStatusEnum.Deleted)
                throw new Exception("Todo item not found or has been deleted.");

            todoItem.MarkAsDeleted();
            _changes.Add(new TodoItemDeletedEvent(id));
        }

        public TodoItem GetTodoItemById(Guid id)
        {
            var todoItem = _todoItems.SingleOrDefault(t => t.Id == id);
            if (todoItem == null || todoItem.Status == TodoStatusEnum.Deleted)
                throw new Exception("Todo item not found or has been deleted.");

            return todoItem;
        }

        public TodoItem[] GetTodoItems()
        {
            return _todoItems
                .Where(t => t.Status != TodoStatusEnum.Deleted)
                .ToArray();
        }

        public void Apply(ITodoEvent @event)
        {
            switch (@event)
            {
                case TodoItemCreatedEvent e:
                    var createdItem = new TodoItem(e.Id, e.Title, e.Description, e.Status, e.ExpiredDate);
                    _todoItems.Add(createdItem);
                    break;

                case TodoItemUpdatedEvent e:
                    var updatedItem = _todoItems.Single(t => t.Id == e.Id);
                    updatedItem.Update(e.Title, e.Description, e.Status, e.ExpiredDate);
                    break;

                case TodoItemDeletedEvent e:
                    var deletedItem = _todoItems.Single(t => t.Id == e.Id);
                    deletedItem.MarkAsDeleted();
                    break;

                case TodoItemUpdatedStatusEvent e:
                    var updatedItemStatus = _todoItems.Single(t => t.Id == e.Id);
                    updatedItemStatus.UpdateStatus(e.Status);
                    break;
            }
        }

        // Metodo per svuotare la lista degli eventi non commessi
        public void ClearUncommittedChanges()
        {
            _changes.Clear();
        }
    }
}

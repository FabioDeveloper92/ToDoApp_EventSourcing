using TodoApp.Domain.Entities;
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

        public void CreateTodoItem(Guid id, string title, string description)
        {
            if (_todoItems.Any(t => t.Id == id))
                throw new Exception("Todo item already exists.");

            var todoItem = new TodoItem(id, title, description);
            _todoItems.Add(todoItem);
            _changes.Add(new TodoItemCreatedEvent(id, title, description));
        }

        public void UpdateTodoItem(Guid id, string title, string description)
        {
            var todoItem = _todoItems.SingleOrDefault(t => t.Id == id);
            if (todoItem == null || todoItem.IsDeleted)
                throw new Exception("Todo item not found or has been deleted.");

            todoItem.Update(title, description);
            _changes.Add(new TodoItemUpdatedEvent(id, title, description));
        }

        public void DeleteTodoItem(Guid id)
        {
            var todoItem = _todoItems.SingleOrDefault(t => t.Id == id);
            if (todoItem == null || todoItem.IsDeleted)
                throw new Exception("Todo item not found or has been deleted.");

            todoItem.MarkAsDeleted();
            _changes.Add(new TodoItemDeletedEvent(id));
        }

        public TodoItem GetTodoItemById(Guid id)
        {
            var todoItem = _todoItems.SingleOrDefault(t => t.Id == id);
            if (todoItem == null || todoItem.IsDeleted)
                throw new Exception("Todo item not found or has been deleted.");

            return todoItem;
        }

        public void Apply(ITodoEvent @event)
        {
            switch (@event)
            {
                case TodoItemCreatedEvent e:
                    var createdItem = new TodoItem(e.Id, e.Title, e.Description);
                    _todoItems.Add(createdItem);
                    break;

                case TodoItemUpdatedEvent e:
                    var updatedItem = _todoItems.Single(t => t.Id == e.Id);
                    updatedItem.Update(e.Title, e.Description);
                    break;

                case TodoItemDeletedEvent e:
                    var deletedItem = _todoItems.Single(t => t.Id == e.Id);
                    deletedItem.MarkAsDeleted();
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

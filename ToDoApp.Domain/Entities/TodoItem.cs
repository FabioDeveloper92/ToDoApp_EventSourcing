namespace TodoApp.Domain.Entities
{
    public class TodoItem
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsDeleted { get; private set; }

        public TodoItem(Guid id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
            IsDeleted = false;
        }

        public void Update(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }
    }

}

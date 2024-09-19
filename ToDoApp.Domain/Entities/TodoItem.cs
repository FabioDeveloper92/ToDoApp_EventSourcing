using TodoApp.Domain.Enum;

namespace TodoApp.Domain.Entities
{
    public class TodoItem
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public TodoStatusEnum Status { get; private set; }
        public DateTime? ExpiredDate { get; private set; }

        public TodoItem(Guid id, string title, string description, TodoStatusEnum status, DateTime? expiredDate)
        {
            Id = id;
            Title = title;
            Description = description;
            Status = status;
            ExpiredDate = expiredDate;
        }

        public void Update(string title, string description, TodoStatusEnum status, DateTime? expiredDate)
        {
            Title = title;
            Description = description;
            Status = status;
            ExpiredDate = expiredDate;
        }

        public void UpdateStatus(TodoStatusEnum status)
        {
            Status = status;
        }

        public void MarkAsDeleted()
        {
            UpdateStatus(TodoStatusEnum.Deleted);
        }
    }

}

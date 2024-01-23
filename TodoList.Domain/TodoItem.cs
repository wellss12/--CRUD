using TodoList.Domain.Enums;

namespace TodoList.Domain;

public class TodoItem
{
    public TodoItem(Guid id, string title, Priority priority, Guid listId)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title is required");
        }

        Id = id;
        Title = title;
        Priority = priority;
        ListId = listId;
    }
    public TodoItem(string title, Priority priority, Guid listId): this(Guid.NewGuid(), title, priority, listId)
    {
    }

    public Guid Id { get; }
    public string Title { get; set; }
    public Priority Priority { get; set; }
    public DateOnly? DueDate { get; set; }
    public Guid ListId { get; set; }
}
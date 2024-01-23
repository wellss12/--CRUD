namespace TodoList.Domain;

public class TodoList
{
    private readonly List<TodoItem> _todoItems = new();

    public TodoList(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new Exception("Title is required");
        }

        Id = Guid.NewGuid();
        Title = title;
    }

    public Guid Id { get; }
    public string Title { get; private set; }

    public IReadOnlyList<TodoItem> TodoItems => _todoItems;

    public void UpdateTitle(string title) => Title = title;

    public void AddItem(TodoItem item) => _todoItems.Add(item);

    public void RemoveItem(TodoItem item) => _todoItems.Remove(item);
}
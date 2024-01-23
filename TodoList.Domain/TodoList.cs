namespace TodoList.Domain;

public class TodoList
{
    private readonly List<TodoItem> _todoItems = new();

    public TodoList(Guid id, string title) 
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title is required");
        }

        Id = id;
        Title = title;
    }

    public TodoList(string title) : this(Guid.NewGuid(), title)
    {
    }

    public Guid Id { get; }
    public string Title { get; private set; }

    public IReadOnlyList<TodoItem> TodoItems => _todoItems;

    public void UpdateTitle(string title) => Title = title;

    public void AddItem(TodoItem item) => _todoItems.Add(item);

    public void RemoveItem(TodoItem item) => _todoItems.Remove(item);

    public void AddItems(IEnumerable<TodoItem> todoItems)
    {
        _todoItems.AddRange(todoItems);
    }
}
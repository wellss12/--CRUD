namespace TodoList.Domain;

public class TodoList
{
    public TodoList(string title)
    {
        Id = Guid.NewGuid();
        Title = title;
    }
    public Guid Id { get; }
    public string Title { get; private set; }

    public void UpdateTitle(string title) => Title = title;
}
namespace TodoList.Infrastructure.DataModels;

public class TodoItemDataModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int Priority { get; set; }
    public string? DueDate { get; set; }
    public string TodoListId { get; set; }
}
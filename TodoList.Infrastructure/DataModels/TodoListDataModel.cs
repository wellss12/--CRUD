namespace TodoList.Infrastructure.DataModels;

public class TodoListDataModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public ICollection<TodoItemDataModel> TodoItems { get; set; }
}
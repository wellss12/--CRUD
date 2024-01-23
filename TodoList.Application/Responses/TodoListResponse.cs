namespace TodoList.Application.Responses;

public class TodoListResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public IEnumerable<TodoItemResponse> TodoItems { get; set; }
}
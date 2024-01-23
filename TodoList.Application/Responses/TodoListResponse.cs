using System.Diagnostics;

namespace TodoList.Application.Responses;

public class TodoListResponse
{
    public Guid Id { get; set; }
    public Process Process { get; set; }
    public IEnumerable<TodoItemResponse> TodoItems { get; set; }
}
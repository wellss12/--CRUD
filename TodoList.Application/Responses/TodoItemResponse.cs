using TodoList.Domain.Enums;

namespace TodoList.Application.Responses;

public class TodoItemResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateOnly? DueDate { get; set; }
    public Priority Priority { get; set; }
}
namespace TodoList.Domain.Exceptions;

public class TodoItemNotFoundException : DomainException
{
    public TodoItemNotFoundException(Guid itemId):base($"{nameof(TodoItem)} {itemId} not found")
    {
    }
}
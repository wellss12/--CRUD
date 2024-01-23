namespace TodoList.Domain.Exceptions;

public class TodoListNotFoundException : DomainException
{
    public TodoListNotFoundException(Guid id) : base($"{nameof(TodoList)} {id} not found")
    {
    }
}
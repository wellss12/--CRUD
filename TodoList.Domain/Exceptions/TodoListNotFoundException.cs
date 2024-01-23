namespace TodoList.Domain.Exceptions;

public class TodoListNotFoundException : DomainException
{
    public TodoListNotFoundException(Guid id) : base($"Todo list with id {id} not found")
    {
    }
}
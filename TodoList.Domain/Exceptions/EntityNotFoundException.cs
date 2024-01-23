namespace TodoList.Domain.Exceptions;

public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityName, Guid id) : base($"{entityName} {id} not found")
    {
    }
}
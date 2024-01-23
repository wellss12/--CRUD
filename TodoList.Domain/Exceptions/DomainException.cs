using System.Net;

namespace TodoList.Domain.Exceptions;

public class DomainException : Exception
{
    public int StatusCode => (int)HttpStatusCode.BadRequest;
    protected DomainException(string message) : base(message)
    {
    }
}
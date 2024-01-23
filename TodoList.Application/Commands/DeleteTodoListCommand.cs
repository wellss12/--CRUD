using MediatR;
using TodoList.Domain;
using TodoList.Domain.Exceptions;

namespace TodoList.Application.Commands;

public record DeleteTodoListCommand(Guid Id): IRequest;

public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand>
{
    private readonly ITodoListRepository _repository;

    public DeleteTodoListCommandHandler(ITodoListRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteTodoListCommand command, CancellationToken cancellationToken)
    {
        var todoList = _repository.Get(command.Id);
        if (todoList is null)
        {
            throw new TodoListNotFoundException(command.Id);
        }

        _repository.Delete(todoList.Id);
    }
}
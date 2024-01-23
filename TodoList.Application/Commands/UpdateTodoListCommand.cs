using MediatR;
using TodoList.Domain;
using TodoList.Domain.Exceptions;

namespace TodoList.Application.Commands;

public record UpdateTodoListCommand(Guid Id, string Title) : IRequest;

public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand>
{
    private readonly ITodoListRepository _repository;

    public UpdateTodoListCommandHandler(ITodoListRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateTodoListCommand command, CancellationToken cancellationToken)
    {
        var todoList = _repository.Get(command.Id);
        if (todoList is null)
        {
            throw new EntityNotFoundException(nameof(Domain.TodoList), command.Id);
        }

        todoList.UpdateTitle(command.Title);
        await _repository.Update(todoList);
    }
}
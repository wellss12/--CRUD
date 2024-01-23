using MediatR;
using TodoList.Domain;
using TodoList.Domain.Exceptions;

namespace TodoList.Application.Commands;

public record RemoveTodoItemCommand(Guid ListId, Guid ItemId) : IRequest;

public class RemoveTodoItemCommandHandler : IRequestHandler<RemoveTodoItemCommand>
{
    private readonly ITodoListRepository _repository;

    public RemoveTodoItemCommandHandler(ITodoListRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveTodoItemCommand command, CancellationToken cancellationToken)
    {
        var todoList = _repository.Get(command.ListId);
        if (todoList is null)
        {
            throw new TodoListNotFoundException(command.ListId);
        }

        var todoItem = todoList.TodoItems.FirstOrDefault(x => x.Id == command.ItemId);
        if (todoItem is null)
        {
            throw new TodoItemNotFoundException(command.ItemId);
        }

        todoList.RemoveItem(todoItem);
        _repository.Update(todoList);
    }
}
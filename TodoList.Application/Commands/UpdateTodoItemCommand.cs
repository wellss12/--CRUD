using MediatR;
using TodoList.Domain;
using TodoList.Domain.Enums;
using TodoList.Domain.Exceptions;

namespace TodoList.Application.Commands;

public class UpdateTodoItemCommand : IRequest
{
    public Guid ListId { get; set; }
    public Guid ItemId { get; set; }
    public string? Title { get; set; }
    public Priority Priority { get; set; }
    public DateOnly? DueDate { get; set; }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand>
{
    private readonly ITodoListRepository _repository;

    public UpdateTodoItemCommandHandler(ITodoListRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateTodoItemCommand command, CancellationToken cancellationToken)
    {
        var todoList = _repository.Get(command.ListId);
        if (todoList is null)
        {
            throw new EntityNotFoundException(nameof(Domain.TodoList), command.ListId);
        }

        var todoItem = todoList.TodoItems.FirstOrDefault(x => x.Id == command.ItemId);
        if (todoItem is null)
        {
            throw new EntityNotFoundException(nameof(TodoItem), command.ItemId);
        }

        todoItem.Priority = command.Priority;
        if (command.Title is not null)
        {
            todoItem.Title = command.Title;
        }

        if (command.DueDate.HasValue)
        {
            todoItem.DueDate = command.DueDate;
        }

        _repository.Update(todoList);
    }
}
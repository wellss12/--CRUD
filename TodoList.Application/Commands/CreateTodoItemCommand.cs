using MediatR;
using TodoList.Domain;
using TodoList.Domain.Enums;
using TodoList.Domain.Exceptions;

namespace TodoList.Application.Commands;

public class CreateTodoItemCommand: IRequest<Guid>
{
    public Guid ListId { get; set; }
    public string Title { get; set; }
    public Priority Priority { get; set; }
    public DateOnly? DueDate { get; set; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, Guid>
{
    private readonly ITodoListRepository _repository;

    public CreateTodoItemCommandHandler(ITodoListRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateTodoItemCommand command, CancellationToken cancellationToken)
    {
        var todoList = _repository.Get(command.ListId);
        if (todoList is null)
        {
            throw new TodoListNotFoundException(command.ListId);
        }

        var todoItem = new TodoItem(command.Title, command.Priority, command.ListId);
        if (command.DueDate.HasValue)
        {
            todoItem.DueDate = command.DueDate;
        }
        
        todoList.AddItem(todoItem);
        _repository.Update(todoList);
        return todoItem.Id;
    }
}
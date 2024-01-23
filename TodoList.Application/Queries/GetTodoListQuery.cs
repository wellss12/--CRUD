using MediatR;
using TodoList.Application.Responses;
using TodoList.Domain;
using TodoList.Domain.Exceptions;

namespace TodoList.Application.Queries;

public record GetTodoListQuery(Guid Id) : IRequest<TodoListResponse>;

public class GetTodoListQueryHandler : IRequestHandler<GetTodoListQuery, TodoListResponse>
{
    private readonly ITodoListRepository _repository;

    public GetTodoListQueryHandler(ITodoListRepository repository)
    {
        _repository = repository;
    }

    public async Task<TodoListResponse> Handle(GetTodoListQuery query, CancellationToken cancellationToken)
    {
        var todoList = _repository.Get(query.Id);
        if (todoList is null)
        {
            throw new EntityNotFoundException(nameof(Domain.TodoList), query.Id);
        }
        
        return new TodoListResponse
        {
            Id = todoList.Id,
            Title = todoList.Title,
            TodoItems = todoList.TodoItems.Select(x => new TodoItemResponse
            {
                Id = x.Id,
                Title = x.Title,
                DueDate = x.DueDate,
                Priority = x.Priority
            })
        };
    }
}
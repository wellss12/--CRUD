using MediatR;
using TodoList.Application.Responses;

namespace TodoList.Application.Queries;

public record GetTodoListQuery(Guid Id) : IRequest<TodoListResponse>;

public class GetTodoListQueryHandler : IRequestHandler<GetTodoListQuery, TodoListResponse>
{
    public async Task<TodoListResponse> Handle(GetTodoListQuery query, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new TodoListResponse { Id = query.Id });
    }
}
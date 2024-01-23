﻿using MediatR;
using TodoList.Application.Responses;
using TodoList.Domain;

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
        
        return new TodoListResponse
        {
            Id = todoList.Id,
            Title = todoList.Title
        };
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Queries;
using TodoList.Application.Responses;

namespace TodoList.Presentation.Controllers;

[ApiController]
[Route("api/todo-list")]
public class TodoListController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodoListController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:Guid}")]
    public async Task<TodoListResponse> Get(Guid id) => await _mediator.Send(new GetTodoListQuery(id));
}
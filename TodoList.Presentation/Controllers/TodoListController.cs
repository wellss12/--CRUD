using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Commands;
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
    public async Task<TodoListResponse> Get(Guid id)
    {
        return await _mediator.Send(new GetTodoListQuery(id));
    }

    [HttpPost]
    public async Task<Guid> Create([FromBody] CreateTodoListCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPut]
    public async Task Update([FromBody] UpdateTodoListCommand command)
    {
        await _mediator.Send(command);
    }
    
    [HttpDelete("{id:Guid}")]
    public async Task Delete(Guid id)
    {
        await _mediator.Send(new DeleteTodoListCommand(id));
    }
}
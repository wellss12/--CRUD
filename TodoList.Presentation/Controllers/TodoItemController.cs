using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Commands;

namespace TodoList.Presentation.Controllers;

[ApiController]
[Route("api/todo-list/{listId:guid}/todo-item")]
public class TodoItemController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodoItemController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Guid> Create(Guid listId, [FromBody] CreateTodoItemCommand command)
    {
        command.ListId = listId;
        return await _mediator.Send(command);
    }
    
    [HttpPut("{itemId:guid}")]
    public async Task Update(Guid listId, Guid itemId, [FromBody] UpdateTodoItemCommand command)
    {
        command.ListId = listId;
        command.ItemId = itemId;
        await _mediator.Send(command);
    }
    
    [HttpDelete("{itemId:guid}")]
    public async Task Delete(Guid listId, Guid itemId)
    {
        await _mediator.Send(new RemoveTodoItemCommand(listId, itemId));
    }
}
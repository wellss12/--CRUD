using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Enums;
using TodoList.Infrastructure.DataModels;
using DM = TodoList.Domain;

namespace TodoList.Infrastructure;

public class TodoListRepository : DM.ITodoListRepository
{
    private readonly TodoListDbContext _dbContext;

    public TodoListRepository(TodoListDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public DM.TodoList? Get(Guid listId)
    {
        var dateModel = _dbContext.TodoLists
            .Include(t => t.TodoItems)
            .AsNoTracking()
            .FirstOrDefault(t => t.Id == listId.ToString());

        if (dateModel is null)
        {
            return null;
        }

        var todoList = new DM.TodoList(Guid.Parse(dateModel.Id), dateModel.Title);
        var todoItems = dateModel.TodoItems.Select(t =>
        {
            var itemId = Guid.Parse(t.Id);
            var priority = (Priority)t.Priority;
            return new DM.TodoItem(itemId, t.Title, priority, todoList.Id);
        });

        todoList.AddItems(todoItems);
        return todoList;
    }

    public async Task Create(DM.TodoList todoList)
    {
        await _dbContext.TodoLists.AddAsync(new TodoListDataModel
        {
            Id = todoList.Id.ToString(),
            Title = todoList.Title
        });
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(DM.TodoList todoList)
    {
        var dataModel = new TodoListDataModel
        {
            Id = todoList.Id.ToString(),
            Title = todoList.Title,
            TodoItems = todoList.TodoItems.Select(t => new TodoItemDataModel
            {
                Id = t.Id.ToString(),
                Title = t.Title,
                Priority = (int)t.Priority,
                DueDate = t.DueDate?.ToString(),
                TodoListId = todoList.Id.ToString()
            }).ToList()
        };

        _dbContext.TodoLists.Update(dataModel);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(DM.TodoList listId)
    {
        var dataModel = new TodoListDataModel
        {
            Id = listId.Id.ToString()
        };

        _dbContext.TodoLists.Remove(dataModel);
        await _dbContext.SaveChangesAsync();
    }
}
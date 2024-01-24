using DM = TodoList.Domain;

namespace TodoList.Infrastructure;

public class InMemoryRepository : DM.ITodoListRepository
{
    private static readonly Dictionary<Guid, Domain.TodoList> TodoListMap = new();

    public DM.TodoList? Get(Guid listId)
    {
        if (TodoListMap.TryGetValue(listId, out var todoList))
        {
            return todoList;
        }

        return null;
    }

    public Task Create(DM.TodoList todoList)
    {
        TodoListMap.Add(todoList.Id, todoList);
        return Task.CompletedTask;
    }

    public Task Update(DM.TodoList todoList)
    {
        TodoListMap[todoList.Id] = todoList;
        return Task.CompletedTask;
    }

    public Task Delete(DM.TodoList todoList)
    {
        TodoListMap.Remove(todoList.Id);
        return Task.CompletedTask;
    }
}
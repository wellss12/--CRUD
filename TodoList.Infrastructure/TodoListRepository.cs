using DM = TodoList.Domain;

namespace TodoList.Infrastructure;

public class TodoListRepository : DM.ITodoListRepository
{
    private readonly Dictionary<Guid, DM.TodoList> _todoListMap = new();

    public DM.TodoList? Get(Guid listId)
    {
        var isExists = _todoListMap.TryGetValue(listId, out var todoList);
        return isExists ? todoList : null;
    }

    public void Create(DM.TodoList todoList)
    {
        if (!_todoListMap.TryAdd(todoList.Id, todoList))
        {
            throw new InvalidOperationException($"Todo list with id {todoList.Id} already exists");
        }
    }

    public void Update(DM.TodoList todoList)
    {
        _todoListMap[todoList.Id] = todoList;
    }
}
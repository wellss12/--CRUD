namespace TodoList.Domain;

public interface ITodoListRepository
{
    TodoList? Get(Guid listId);
    Task Create(TodoList todoList);
    Task Update(TodoList todoList);
    Task Delete(TodoList listId);
}
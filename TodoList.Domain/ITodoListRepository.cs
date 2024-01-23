namespace TodoList.Domain;

public interface ITodoListRepository
{
    TodoList? Get(Guid listId);
    void Create(TodoList todoList);
    void Update(TodoList todoList);
    void Delete(Guid listId);
}
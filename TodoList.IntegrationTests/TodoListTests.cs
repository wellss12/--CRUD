using System.Net.Http.Json;
using FluentAssertions;
using TodoList.Application.Commands;
using TodoList.Application.Responses;
using TodoList.Domain;
using TodoList.Domain.Enums;

namespace TodoList.IntegrationTests;

public class TodoListTests
{
    private IntegrationTestServer _server = null!;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _server = new IntegrationTestServer();
    }

    [Category("TodoList")]
    [Test]
    public async Task Get_Todo_List()
    {
        var repository = _server.GetRequiredService<ITodoListRepository>();
        var todoList = new Domain.TodoList("Todo");
        var todoItem = new TodoItem("TodoItem", Priority.Low, todoList.Id);
        todoList.AddItem(todoItem);
        repository.Create(todoList);

        var result = await _server.Client.GetAsync($"api/todo-list/{todoList.Id}");

        result.IsSuccessStatusCode.Should().BeTrue();
        var content = await result.Content.ReadFromJsonAsync<TodoListResponse>();
        content.Id.Should().Be(todoList.Id);
        content.Title.Should().Be(todoList.Title);
        content.TodoItems.Count().Should().Be(1);
        content.TodoItems.Should().BeEquivalentTo(new List<TodoItemResponse>()
        {
            new()
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                DueDate = todoItem.DueDate,
                Priority = todoItem.Priority
            }
        });
    }

    [Category("TodoList")]
    [Test]
    public async Task Create_Todo_List()
    {
        const string title = "Todo";
        var jsonContent = JsonContent.Create(new CreateTodoListCommand(title));
        var response = await _server.Client.PostAsync("api/todo-list", jsonContent);

        response.IsSuccessStatusCode.Should().BeTrue();
        var listId = await response.Content.ReadFromJsonAsync<Guid>();
        listId.Should().NotBeEmpty();

        var repository = _server.GetRequiredService<ITodoListRepository>();
        var todoList = repository.Get(listId);
        todoList.Should().NotBeNull();
        todoList.Id.Should().Be(listId);
        todoList.Title.Should().Be(title);
    }

    [Category("TodoList")]
    [Test]
    public async Task Update_Todo_List()
    {
        var repository = _server.GetRequiredService<ITodoListRepository>();
        var original = new Domain.TodoList("OriginalTodo");
        repository.Create(original);

        const string targetTitle = "TargetTodo";
        var jsonContent = JsonContent.Create(new UpdateTodoListCommand(original.Id, targetTitle));
        var response = await _server.Client.PutAsync("api/todo-list", jsonContent);

        response.IsSuccessStatusCode.Should().BeTrue();
        var todoList = repository.Get(original.Id);
        todoList.Should().NotBeNull();
        todoList.Id.Should().Be(original.Id);
        todoList.Title.Should().Be(targetTitle);
    }

    [Category("TodoList")]
    [Test]
    public async Task Delete_Todo_List()
    {
        var repository = _server.GetRequiredService<ITodoListRepository>();
        var beforeTodoList = new Domain.TodoList("Todo");
        repository.Create(beforeTodoList);

        var response = await _server.Client.DeleteAsync($"api/todo-list/{beforeTodoList.Id}");

        response.IsSuccessStatusCode.Should().BeTrue();
        var afterTodoList = repository.Get(beforeTodoList.Id);
        afterTodoList.Should().BeNull();
    }

    [Category("TodoItem")]
    [Test]
    public async Task Create_Todo_Item()
    {
        // Arrange
        var repository = _server.GetRequiredService<ITodoListRepository>();
        var beforeTodoList = new Domain.TodoList("Todo");
        repository.Create(beforeTodoList);

        var todoListId = beforeTodoList.Id;
        var command = new CreateTodoItemCommand()
        {
            Title = "TodoItem",
            Priority = Priority.Medium,
            DueDate = new DateOnly(2024, 2, 20)
        };
        var jsonContent = JsonContent.Create(command);

        // Act
        var response = await _server.Client.PostAsync($"api/todo-list/{todoListId}/todo-item", jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var todoItemId = await response.Content.ReadFromJsonAsync<Guid>();
        todoItemId.Should().NotBeEmpty();

        var afterTodoList = repository.Get(todoListId)!;
        var todoItems = afterTodoList.TodoItems;
        todoItems.Count.Should().Be(1);
        var todoItem = todoItems[0];
        todoItem.Id.Should().Be(todoItemId);
        todoItem.Title.Should().Be(command.Title);
        todoItem.Priority.Should().Be(command.Priority);
        todoItem.DueDate.Should().Be(command.DueDate);
    }

    [Category("TodoItem")]
    [Test]
    public async Task Update_Todo_Item()
    {
        // Arrange
        var repository = _server.GetRequiredService<ITodoListRepository>();
        var beforeTodoList = new Domain.TodoList("Todo");
        var beforeTodoItem = new TodoItem("TodoItem", Priority.Low, beforeTodoList.Id);
        beforeTodoList.AddItem(beforeTodoItem);
        repository.Create(beforeTodoList);

        var command = new UpdateTodoItemCommand()
        {
            Title = "TitleChanged",
            Priority = Priority.High,
            DueDate = new DateOnly(2024, 2, 20)
        };
        var jsonContent = JsonContent.Create(command);

        // Act
        var response = await _server.Client.PutAsync($"api/todo-list/{beforeTodoList.Id}/todo-item/{beforeTodoItem.Id}",
            jsonContent);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        var afterTodoItem = repository.Get(beforeTodoList.Id)!.TodoItems[0];
        afterTodoItem.Title.Should().Be(command.Title);
        afterTodoItem.Priority.Should().Be(command.Priority);
        afterTodoItem.DueDate.Should().Be(command.DueDate);
    }
    
    [Category("TodoItem")]
    [Test]
    public async Task Remove_Todo_Item()
    {
        var repository = _server.GetRequiredService<ITodoListRepository>();
        var beforeTodoList = new Domain.TodoList("Todo");
        var beforeTodoItem = new TodoItem("TodoItem", Priority.Low, beforeTodoList.Id);
        beforeTodoList.AddItem(beforeTodoItem);
        repository.Create(beforeTodoList);

        var response = await _server.Client.DeleteAsync($"api/todo-list/{beforeTodoList.Id}/todo-item/{beforeTodoItem.Id}");

        response.IsSuccessStatusCode.Should().BeTrue();
        repository.Get(beforeTodoList.Id)!.TodoItems.Should().BeEmpty();
    }
}
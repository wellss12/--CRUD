using System.Net.Http.Json;
using FluentAssertions;
using TodoList.Application.Commands;
using TodoList.Application.Responses;
using TodoList.Domain;

namespace TodoList.IntegrationTests;

public class TodoListTests
{
    private IntegrationTestServer _server = null!;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _server = new IntegrationTestServer();
    }

    [Test]
    public async Task Get_Todo_List()
    {
        var listId = Guid.NewGuid();
        var result = await _server.Client.GetAsync($"api/todo-list/{listId}");

        result.IsSuccessStatusCode.Should().BeTrue();
        var content = await result.Content.ReadFromJsonAsync<TodoListResponse>();
        content.Id.Should().Be(listId);
    }

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
}
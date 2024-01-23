using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
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
}
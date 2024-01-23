using System.Net.Http.Json;
using FluentAssertions;
using TodoList.Application.Responses;

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
}
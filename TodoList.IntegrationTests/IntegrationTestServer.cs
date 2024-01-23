using Microsoft.AspNetCore.Mvc.Testing;

namespace TodoList.IntegrationTests;

public class IntegrationTestServer : WebApplicationFactory<Program>
{
    public HttpClient Client { get; }

    public IntegrationTestServer()
    {
        Client = CreateClient();
    }
}
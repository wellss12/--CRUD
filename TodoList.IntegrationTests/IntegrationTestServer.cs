using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace TodoList.IntegrationTests;

public class IntegrationTestServer : WebApplicationFactory<Program>
{
    public HttpClient Client { get; }

    public IntegrationTestServer()
    {
        Client = CreateClient();
    }

    public T GetRequiredService<T>() where T : notnull
        => Services.CreateScope().ServiceProvider.GetRequiredService<T>();
}
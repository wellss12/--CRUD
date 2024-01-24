using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Domain;
using TodoList.Infrastructure;

namespace TodoList.IntegrationTests;

public class IntegrationTestServer : WebApplicationFactory<Program>
{
    public HttpClient Client { get; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddScoped<ITodoListRepository, InMemoryRepository>();
        });
    }

    public IntegrationTestServer()
    {
        Client = CreateClient();
    }

    public T GetRequiredService<T>() where T : notnull
        => Services.CreateScope().ServiceProvider.GetRequiredService<T>();
}
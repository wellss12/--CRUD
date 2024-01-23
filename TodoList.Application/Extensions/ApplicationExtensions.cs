using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TodoList.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}
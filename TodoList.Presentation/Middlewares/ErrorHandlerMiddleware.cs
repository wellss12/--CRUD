using System.Net;
using System.Text.Json;
using TodoList.Domain.Exceptions;

namespace TodoList.Presentation.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain Exception");
            await HandleException(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "System Exception");
            await HandleException(context, ex);
        }
    }

    private static Task HandleException(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = GetStatusCode(exception);

        var json = JsonSerializer.Serialize(new DefaultResponse
        {
            Message = exception.Message
        });

        return context.Response.WriteAsync(json);
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest,
            DomainException domainException => domainException.StatusCode,
            _ => (int)HttpStatusCode.InternalServerError
        };
}
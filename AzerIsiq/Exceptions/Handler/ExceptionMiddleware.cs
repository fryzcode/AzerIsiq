using System.Net;
using System.Text.Json;
using FluentValidation;

namespace AzerIsiq.Extensions.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var statusCode = exception switch
        {
            NotFoundException => (int)HttpStatusCode.NotFound,
            // ValidationException => (int)HttpStatusCode.BadRequest,
            NotAuthorizedException => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
        };
        
        context.Response.StatusCode = statusCode;

        var response = new { message = exception.Message, statusCode = statusCode };
        var result = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(result);
    }
}

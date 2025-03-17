using System.Net;
using System.Text.Json;
using FluentValidation;

namespace AzerIsiq.Extensions.Exceptions;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex) 
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var errors = new
            {
                message = "Validation failed",
                statusCode = 400,
                errors = ex.Errors.Select(error => new
                {
                    field = error.PropertyName,
                    message = error.ErrorMessage,
                    severity = error.Severity.ToString()
                })
            };

            var json = JsonSerializer.Serialize(errors);
            await context.Response.WriteAsync(json);
        }
    }
}
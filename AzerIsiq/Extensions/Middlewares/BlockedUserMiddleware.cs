using System.Security.Claims;
using System.Text.Json;
using AzerIsiq.Services.ILogic;
using AzerIsiq.Dtos;

namespace AzerIsiq.Extensions.Middlewares;

public class BlockedUserMiddleware
{
    private readonly RequestDelegate _next;

    public BlockedUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var user = await userService.GetUserByIdAsync(userId);
                if (user != null && user.IsBlocked)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";

                    var errorResponse = ApiResponse<string>.Fail(
                        statusCode: 403,
                        message: "Your account is blocked.",
                        path: context.Request.Path
                    );

                    var json = JsonSerializer.Serialize(errorResponse);
                    await context.Response.WriteAsync(json);
                    return;
                }
            }
        }

        await _next(context);
    }
}
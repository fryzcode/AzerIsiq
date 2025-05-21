using ChatSystem;
using ChatSystem.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsPolicies(); // ✅ Подключение CORS
builder.Services.AddApplicationServices(builder.Configuration); // ✅ Подключение остальных сервисов

var app = builder.Build();

app.UseRouting();

app.UseCors(); // Или app.UseCors("SignalRPolicy"); если нужна конкретная политика

app.UseAuthentication();
app.UseAuthorization();
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        Console.WriteLine("CORS headers:");
        foreach (var h in context.Response.Headers)
            Console.WriteLine($"{h.Key}: {h.Value}");
        return Task.CompletedTask;
    });
    await next();
});

app.MapControllers();
app.MapHub<ChatHub>("/chathub").RequireCors("SignalRPolicy");

app.Run();
using ChatSystem;
using ChatSystem.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsPolicies(); 
builder.Services.AddApplicationServices(builder.Configuration);

var kestrelPort = builder.Configuration.GetValue<int>("Kestrel:EndpointPort");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(kestrelPort);
});

Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"DBConnection: {builder.Configuration.GetConnectionString("DefaultConnection")}");
Console.WriteLine($"Kestrel Port: {kestrelPort}");

var app = builder.Build();

app.UseRouting();

app.UseCors(); 

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
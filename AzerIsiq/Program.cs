using AzerIsiq.Extensions;
using AzerIsiq.Extensions.DbInit;
using AzerIsiq.Extensions.Exceptions;
using AzerIsiq.Extensions.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCorsPolicy()
    .AddDatabaseConfiguration(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddApplicationServices();

builder.Services.AddSignalR();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var kestrelPort = builder.Configuration.GetValue<int>("Kestrel:EndpointPort");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(kestrelPort);
});

Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"DBConnection: {builder.Configuration.GetConnectionString("DBConnection")}");
Console.WriteLine($"Kestrel Port: {kestrelPort}");

var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
//     dbInitializer.Initialize();
// }
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<ValidationExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("OpenPolicy");
// app.MapHub<ChatHub>("/chathub").RequireCors("AllowAll");;

app.UseAuthentication();
app.UseMiddleware<BlockedUserMiddleware>();
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
app.MapHub<ChatHub>("/chathub").RequireCors("SignalRPolicy");

// app.UseHttpMetrics(); Prometheus
app.MapControllers();
// app.MapMetrics();     Prometheus

app.Run();
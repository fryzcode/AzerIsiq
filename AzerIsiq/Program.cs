using AzerIsiq.Extensions;
using AzerIsiq.Extensions.DbInit;
using AzerIsiq.Extensions.Exceptions;
using AzerIsiq.Extensions.Middlewares;
using AzerIsiq.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR().AddHubOptions<ChatHub>(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
});

builder.Services
    // .AddCorsPolicy()
    .AddDatabaseConfiguration(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddApplicationServices();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
builder.Services.AddGrpc();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var kestrelApi = builder.Configuration.GetValue<int>("Kestrel-API:EndpointPort");
var kestrelGrpc = builder.Configuration.GetValue<int>("Kestrel-GRPC:EndpointPort");

builder.WebHost.ConfigureKestrel(options =>
{
    var env = builder.Environment.EnvironmentName;

    if (env == "Development")
    {
        // Local
        options.ListenLocalhost(kestrelApi, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1;
        });

        options.ListenLocalhost(kestrelGrpc, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http2;
        });
    }
    else
    {
        // Production (Docker)
        options.ListenAnyIP(kestrelApi, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1;
        });

        options.ListenAnyIP(kestrelGrpc, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http2;
        });
    }
});


// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.ListenAnyIP(kestrelPort, listenOptions =>
//     {
//         listenOptions.Protocols = HttpProtocols.Http2;
//     });
// });

Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"DBConnection: {builder.Configuration.GetConnectionString("DBConnection")}");
Console.WriteLine($"Kestrel Port API: {kestrelApi}");
Console.WriteLine($"Kestrel Port GRPC: {kestrelGrpc}");

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
// app.UseHttpsRedirection();
app.UseRouting();
// app.UseCors("OpenPolicy");
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
// app.MapHub<ChatHub>("/chathub").RequireCors("SignalRPolicy");

app.MapGrpcService<UserGrpcServiceImpl>();

// app.MapGrpcService<UserGrpcService>();
// app.MapGet("/", () => "gRPC server is running...");
app.MapGet("/", () => "Hello from AzerIsiq gRPC!");

// app.UseHttpMetrics(); Prometheus
app.MapControllers();
// app.MapMetrics();     Prometheus

app.Run();
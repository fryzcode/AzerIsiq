using System.Security.Claims;
using System.Text;
using Azerisiq.Grpc;
using ChatSystem.Abstraction;
using ChatSystem.Data;
using ChatSystem.Services;
using GrpcService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ChatSystem
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

            services.AddJwtAuthentication(config);

            // services.AddAuthorization();
            services.AddSignalR();

            services.AddGrpcClient<UserGrpcService.UserGrpcServiceClient>(o =>
            {
                o.Address = new Uri(config["Grpc:UserServiceUrl"]);
            });

            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<UserGrpcClientService>();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddGrpc();

            return services;
        }
        
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var keyString = jwtSettings["Key"];
            Console.WriteLine($"JWT Raw Key (from config): {jwtSettings["Key"]}");
            Console.WriteLine($"JWT Base64 Key: {Convert.ToBase64String(Encoding.UTF8.GetBytes(jwtSettings["Key"]))}");
            if (string.IsNullOrEmpty(keyString))
            {
                throw new Exception("JWT Key is missing in configuration.");
            }

            var key = Encoding.UTF8.GetBytes(keyString);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // ValidateIssuer = false,
                        // ValidateAudience = false,
                        // ValidateLifetime = true,
                        // ValidateIssuerSigningKey = true,
                        // ValidIssuer = jwtSettings["Issuer"],
                        // ValidAudience = jwtSettings["Audience"],
                        // IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("❌ Token validation failed:");
                            Console.WriteLine(context.Exception.ToString());
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("✅ Token validated successfully");
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                        // OnMessageReceived = context =>
                        // {
                        //     var accessToken = context.Request.Query["access_token"];
                        //     var path = context.HttpContext.Request.Path;
                        //
                        //     if (!string.IsNullOrEmpty(accessToken) &&
                        //         path.StartsWithSegments("/chathub"))
                        //     {
                        //         context.Token = accessToken;
                        //     }
                        //
                        //     return Task.CompletedTask;
                        // }
                    };
                });

            return services;
        }

    }
}

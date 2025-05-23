using AzerIsiq.Data;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Repository.Services;
using AzerIsiq.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using AzerIsiq.Dtos;
using AzerIsiq.Extensions.BackgroundTasks;
using AzerIsiq.Extensions.DbInit;
using AzerIsiq.Extensions.Mapping;
using AzerIsiq.Extensions.Repository;
using AzerIsiq.Hubs.CustomProvider;
using AzerIsiq.Models;
using AzerIsiq.Services.Helpers;
using AzerIsiq.Services.ILogic;
using AzerIsiq.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AzerIsiq.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Db init
            services.AddScoped<IDbInitializer, DbInitializer>();
            // Repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IReadOnlyRepository<Region>, RegionRepository>();
            services.AddScoped<IReadOnlyRepository<District>, DistrictRepository>();
            services.AddScoped<IGenericRepository<Substation>, SubstationRepository>();
            services.AddScoped<IGenericRepository<Tm>, TmRepository>();
            services.AddScoped<IGenericRepository<Location>, LocationRepository>();
            services.AddScoped<ILoggerRepository, LoggerRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<ISubstationRepository, SubstationRepository>();
            services.AddScoped<ITmRepository, TmRepository>();
            services.AddScoped<IOtpCodeRepository, OtpCodeRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<ISubscriberRepository, SubscriberRepository>();
            services.AddScoped<ICounterRepository, CounterRepository>();
            services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<ILoggerRepository, LoggerRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            
            // Services
            services.AddScoped(typeof(IReadOnlyService<>), typeof(ReadOnlyService<>));
            
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<ISubstationService, SubstationService>();
            services.AddScoped<ITmService, TmService>();
            
            services.AddScoped<ISubscriberService, SubscriberService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ICounterService, CounterService>();
            services.AddScoped<ISubscriberCodeGenerator, SubscriberCodeGenerator>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ILoggingService, LoggingService>();
            services.AddScoped<OtpService>();
            services.AddScoped<JwtService>();
            
            services.AddHostedService<FailedAttemptsResetTask>();
            services.AddHostedService<SubscriberDebtService>();
            
            services.AddScoped<IElectronicAppealRepository, ElectronicAppealRepository>();
            services.AddScoped<IElectronicAppealService, ElectronicAppealService>();
            
            services.AddScoped<UserGrpcServiceImpl>();

            services.AddScoped<IChatService, ChatService>();

            services.AddAutoMapper(typeof(Program));

            services.AddHttpContextAccessor();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
        
        // public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        // {
        //     services.AddCors(options =>
        //     {
        //         options.AddPolicy("OpenPolicy", policy =>
        //         {
        //             policy.AllowAnyOrigin()
        //                 .AllowAnyMethod()
        //                 .AllowAnyHeader();
        //         });
        //
        //         options.AddPolicy("SignalRPolicy", policy =>
        //         {
        //             policy.WithOrigins(
        //                     "http://127.0.0.1:5500",
        //                     "http://127.0.0.1:5001",
        //                     "http://localhost:5001",
        //                     "http://localhost:5001",
        //                     "http://localhost:5298",
        //                     "http://localhost:3000",
        //                     "http://127.0.0.1:3000",
        //                     "http://192.168.56.1:3000",
        //                     "http://192.168.1.18:3000",
        //                     // "http://localhost:5500",
        //                     "http://192.168.137.19:3000",
        //                     "https://192.168.137.19:3000"
        //                 )
        //                 .AllowAnyHeader()
        //                 .AllowAnyMethod()
        //                 .AllowCredentials();
        //         });
        //     });
        //
        //     return services;
        // }

        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DBConnection")));

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
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                    
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/chathub"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}

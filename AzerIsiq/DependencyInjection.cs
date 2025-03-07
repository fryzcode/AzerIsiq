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
using AzerIsiq.Models;

namespace AzerIsiq.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
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
            services.AddScoped<IImageService, ImageService>();
            
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<ILocationService, LocationService>();

            services.AddScoped<AuthService>();
            services.AddScoped<JwtService>();
            services.AddScoped<RegionService>();
            services.AddScoped<SubstationService>();
            services.AddScoped<DistrictService>();
            services.AddScoped<TmService>();
            services.AddScoped<OtpService>();
            
            services.AddHttpContextAccessor();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return services;
        }

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
                });

            return services;
        }
    }
}

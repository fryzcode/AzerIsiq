using AzerIsiq.Repository.Interface;
using AzerIsiq.Repository.Services;
using AzerIsiq.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using AzerIsiq.Models;

namespace AzerIsiq.Extensions
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<AuthService>();
            services.AddScoped<JwtService>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IReadOnlyRepository<Region>, RegionRepository>();
            services.AddScoped<IReadOnlyRepository<District>, DistrictRepository>();
            services.AddScoped<IGenericRepository<Substation>, SubstationRepository>();
            services.AddScoped<RegionService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<ISubstationRepository, SubstationRepository>();
            services.AddScoped<ILoggerRepository, LoggerRepository>();

            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<SubstationService>();
            services.AddScoped<DistrictService>();

            services.AddHttpContextAccessor();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
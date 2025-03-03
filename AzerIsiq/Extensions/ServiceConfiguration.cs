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
    }
}
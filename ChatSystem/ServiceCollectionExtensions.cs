using Microsoft.Extensions.DependencyInjection;

namespace ChatSystem
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                // options.AddPolicy("OpenPolicy", policy =>
                // {
                //     policy.AllowAnyOrigin()
                //         .AllowAnyHeader()
                //         .AllowAnyMethod();
                // });

                options.AddPolicy("SignalRPolicy", policy =>
                {
                    policy.WithOrigins(
                            "http://127.0.0.1:5500",
                            "http://localhost:3000",
                            "http://127.0.0.1:3000",
                            "http://192.168.56.1:3000",
                            "http://192.168.1.18:3000",
                            "http://192.168.137.19:3000",
                            "https://192.168.137.19:3000"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}
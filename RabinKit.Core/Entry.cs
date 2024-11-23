using RabinKit.Core.Abstractions;
using RabinKit.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace RabinKit.Core
{
    public static class Entry
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddMediatR(config =>
                config.RegisterServicesFromAssembly(typeof(Entry).Assembly));
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IDbSeeder, DbSeeder>();
            services.AddScoped<TaskRunner>();

            return services;
        }
    }
}
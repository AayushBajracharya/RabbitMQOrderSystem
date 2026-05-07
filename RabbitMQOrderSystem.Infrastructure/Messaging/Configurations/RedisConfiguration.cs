using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RabbitMQOrderSystem.Infrastructure.Messaging.Configurations
{
    public static class RedisConfiguration
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("Redis").Get<RedisSettings>()
                           ?? new RedisSettings();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{settings.Host}:{settings.Port},password={settings.Password},abortConnect=false";
                options.InstanceName = "RabbitMQOrderSystem_";
            });

            return services;
        }
    }
}

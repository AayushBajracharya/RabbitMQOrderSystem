using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQOrderSystem.Application.Interfaces;
using RabbitMQOrderSystem.Infrastructure.Messaging.Configurations;
using RabbitMQOrderSystem.Infrastructure.Messaging.Consumers;
using RabbitMQOrderSystem.Infrastructure.Publishers;
namespace RabbitMQOrderSystem.Infrastructure.Messaging;

public static class RabbitMqConfiguration
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection("RabbitMq").Get<RabbitMqSettings>()
                       ?? new RabbitMqSettings();

        services.AddMassTransit(x =>
        {
            // Register Consumer
            x.AddConsumer<OrderCreatedConsumer>();
            x.UsingRabbitMq((context, cfg) =>
            {
                // Correct Host configuration for MassTransit + RabbitMQ
                cfg.Host(new Uri($"amqp://{settings.Host}:{settings.Port}"), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                // Optional: Set virtual host if not "/"
                // cfg.Host(new Uri($"amqp://{settings.Host}:{settings.Port}{settings.VirtualHost}"), ...

                // Reliability settings (good for mid-level project)
                cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                cfg.UseInMemoryOutbox();

                // Auto configure consumers
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddMassTransitHostedService();
        services.AddScoped<IEventPublisher, EventPublisher>();
        return services;
    }
}
using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMQOrderSystem.Application.Interfaces;
using RabbitMQOrderSystem.Domain.Events;

namespace RabbitMQOrderSystem.Infrastructure.Messaging.Consumers;

public class OrderUpdatedConsumer : IConsumer<OrderUpdatedEvent>
{
    private readonly ILogger<OrderUpdatedConsumer> _logger;
    private readonly ICacheService _cacheService;

    public OrderUpdatedConsumer(
        ILogger<OrderUpdatedConsumer> logger,
        ICacheService cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task Consume(ConsumeContext<OrderUpdatedEvent> context)
    {
        var eventData = context.Message;

        _logger.LogInformation("🔄 Processing OrderUpdatedEvent - OrderId: {OrderId}", eventData.OrderId);

        try
        {
            // Invalidate cache when order is updated
            var cacheKey = $"order:{eventData.OrderId}";
            await _cacheService.RemoveAsync(cacheKey);

            _logger.LogInformation("🗑️ Cache invalidated for Order {OrderId}", eventData.OrderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Failed to process OrderUpdatedEvent for Order {OrderId}", eventData.OrderId);
        }
    }
}
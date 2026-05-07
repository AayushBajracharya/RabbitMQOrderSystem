using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMQOrderSystem.Application.Interfaces;
using RabbitMQOrderSystem.Domain.Events;

namespace RabbitMQOrderSystem.Infrastructure.Messaging.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedConsumer> _logger;
        private readonly ICacheService _cacheService;

        public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger, ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var order = context.Message;

            _logger.LogInformation("=== Processing Order from RabbitMQ ===");
            _logger.LogInformation("Order ID: {OrderId}", order.OrderId);

            await Task.Delay(1500); 

            // Cache the processed order
            var cacheKey = $"order:{order.OrderId}";
            await _cacheService.SetAsync(cacheKey, order, TimeSpan.FromMinutes(60));

            _logger.LogInformation("Order {OrderId} processed and cached successfully!", order.OrderId);
        }
    }
}

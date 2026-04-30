using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMQOrderSystem.Domain.Events;

namespace RabbitMQOrderSystem.Infrastructure.Messaging.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var order = context.Message;

            _logger.LogInformation("=== Order Received from RabbitMQ ===");
            _logger.LogInformation("Order ID     : {OrderId}", order.OrderId);
            _logger.LogInformation("Order Number : {OrderNumber}", order.OrderNumber);
            _logger.LogInformation("Amount       : ${Amount}", order.Amount);
            _logger.LogInformation("Customer     : {CustomerEmail}", order.CustomerEmail);
            _logger.LogInformation("Items Count  : {ItemCount}", order.Items.Count);
            _logger.LogInformation("Received At  : {ReceivedAt}", DateTime.UtcNow);

            // Simulate processing time (e.g., send email, update inventory, etc.)
            await Task.Delay(2000);

            _logger.LogInformation("Order {OrderId} processed successfully!", order.OrderId);

            // You can throw exception here to test retry / DLQ later
        }
    }
}

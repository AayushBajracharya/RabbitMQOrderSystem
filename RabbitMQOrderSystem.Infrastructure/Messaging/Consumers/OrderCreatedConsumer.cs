using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMQOrderSystem.Application.Interfaces;
using RabbitMQOrderSystem.Domain.Entities;
using RabbitMQOrderSystem.Domain.Events;

namespace RabbitMQOrderSystem.Infrastructure.Messaging.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedConsumer> _logger;
    private readonly ICacheService _cacheService;
    private readonly IOrderRepository _orderRepository;

    public OrderCreatedConsumer(
        ILogger<OrderCreatedConsumer> logger,
        ICacheService cacheService,
        IOrderRepository orderRepository)
    {
        _logger = logger;
        _cacheService = cacheService;
        _orderRepository = orderRepository;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var eventData = context.Message;

        _logger.LogInformation("Processing Order {OrderId} from RabbitMQ", eventData.OrderId);

        try
        {
            // Create OrderItems using fully qualified name to avoid conflict
            var orderItems = eventData.Items.Select(i =>
               OrderItem.Create(
                   i.ProductName,
                   i.Quantity,
                   i.UnitPrice)
           ).ToList();

            // Create Order
            var order = Order.Create(
                eventData.CustomerEmail,
                eventData.Amount,
                eventData.Status,
                orderItems);

            // Save to Database
            await _orderRepository.AddAsync(order);

            // Cache the result
            var cacheKey = $"order:{order.Id}";
            await _cacheService.SetAsync(cacheKey, order, TimeSpan.FromHours(2));

            order.MarkAsProcessed();

            _logger.LogInformation("✅ Order {OrderId} saved to DB and cached successfully!", order.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Failed to process order {OrderId}", eventData.OrderId);
        }
    }
}
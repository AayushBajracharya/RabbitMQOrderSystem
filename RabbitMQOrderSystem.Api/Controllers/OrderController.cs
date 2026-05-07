using Microsoft.AspNetCore.Mvc;
using RabbitMQOrderSystem.Api.DTOs;
using RabbitMQOrderSystem.Application.Interfaces;
using RabbitMQOrderSystem.Domain.Events;

namespace RabbitMQOrderSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheService _cacheService;

        public OrderController(IEventPublisher eventPublisher, ICacheService cacheService)
        {
            _eventPublisher = eventPublisher;
            _cacheService = cacheService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var orderEvent = new OrderCreatedEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8]}",
                Amount = request.Amount,
                CustomerEmail = request.CustomerEmail,
                CreatedAt = DateTime.UtcNow,
                Items = request.Items.Select(i => new OrderItem
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            await _eventPublisher.PublishOrderCreatedAsync(orderEvent);

            return Ok(new
            {
                Message = "Order created and event published successfully!",
                OrderId = orderEvent.OrderId,
                OrderNumber = orderEvent.OrderNumber
            });
        }

        [HttpGet("status/{orderId:guid}")]
        public async Task<IActionResult> GetOrderStatus(Guid orderId)
        {
            var cacheKey = $"order:{orderId}";

            // Try from Cache first (Cache-Aside)
            var cachedOrder = await _cacheService.GetAsync<OrderCreatedEvent>(cacheKey);
            if (cachedOrder != null)
            {
                return Ok(new { Source = "Cache", Order = cachedOrder });
            }

            // Cache Miss → In real project you would get from Database
            // For now, we'll return a simulated response
            var order = new OrderCreatedEvent
            {
                OrderId = orderId,
                OrderNumber = $"ORD-SIM-{DateTime.UtcNow:yyyyMMdd}",
                Amount = 1299.99m,
                CustomerEmail = "customer@example.com",
                CreatedAt = DateTime.UtcNow,
                Items = new List<OrderItem> { new OrderItem { ProductName = "Laptop", Quantity = 1, UnitPrice = 1299.99m } }
            };

            // Store in Cache
            await _cacheService.SetAsync(cacheKey, order);

            return Ok(new { Source = "Database (simulated)", Order = order });
        }
    }
}

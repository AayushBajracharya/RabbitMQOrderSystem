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

        public OrderController(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
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
    }
}

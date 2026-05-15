using Microsoft.AspNetCore.Mvc;
using RabbitMQOrderSystem.Api.DTOs;
using RabbitMQOrderSystem.Application.Interfaces;
using RabbitMQOrderSystem.Domain.Entities;
using RabbitMQOrderSystem.Domain.Events;

namespace RabbitMQOrderSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheService _cacheService;
        private readonly IOrderRepository _orderRepository;

        public OrderController(IEventPublisher eventPublisher, ICacheService cacheService, IOrderRepository orderRepository )
        {
            _eventPublisher = eventPublisher;
            _cacheService = cacheService;
            _orderRepository = orderRepository;
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
                Status = request.Status,
                Items = request.Items.Select(i => new OrderItemDto
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

            // 1. Try Cache First (Cache-Aside Pattern)
            var cachedOrder = await _cacheService.GetAsync<Order>(cacheKey);
            if (cachedOrder != null)
            {
                return Ok(new
                {
                    Source = "Redis Cache",
                    Order = cachedOrder
                });
            }

            // 2. Cache Miss → Fetch from Database
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
            {
                return NotFound(new { Message = $"Order with ID {orderId} not found." });
            }

            // 3. Store in Cache for next time
            await _cacheService.SetAsync(cacheKey, order, TimeSpan.FromMinutes(30));

            return Ok(new
            {
                Source = "Database",
                Order = order
            });
        }
    }
}

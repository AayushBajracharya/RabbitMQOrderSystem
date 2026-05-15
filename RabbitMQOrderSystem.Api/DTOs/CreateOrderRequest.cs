using RabbitMQOrderSystem.Domain.Entities;

namespace RabbitMQOrderSystem.Api.DTOs
{
    public class CreateOrderRequest
    {
        public decimal Amount { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public List<OrderItemRequest> Items { get; set; } = new();
    }
}

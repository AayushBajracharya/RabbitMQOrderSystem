using RabbitMQOrderSystem.Domain.Entities;

namespace RabbitMQOrderSystem.Domain.Events;

public record OrderCreatedEvent
{
    public Guid OrderId { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string CustomerEmail { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public List<OrderItemDto> Items { get; init; } = new();
}

public record OrderItemDto
{
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
}
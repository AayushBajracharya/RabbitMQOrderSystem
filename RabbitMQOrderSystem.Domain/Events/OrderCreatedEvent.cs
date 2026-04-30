namespace RabbitMQOrderSystem.Domain.Events;

public record OrderCreatedEvent
{
    public Guid OrderId { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string CustomerEmail { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public List<OrderItem> Items { get; init; } = new();
}

public record OrderItem
{
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
}
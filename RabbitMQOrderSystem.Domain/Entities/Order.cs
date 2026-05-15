namespace RabbitMQOrderSystem.Domain.Entities;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string OrderNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string CustomerEmail { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }

    public List<OrderItem> Items { get; set; } = new();

    public static Order Create(string customerEmail, decimal amount, OrderStatus status, List<OrderItem> items)
    {
        var order = new Order
        {
            OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8]}",
            CustomerEmail = customerEmail,
            Amount = amount,
            Status = status,
            Items = items ?? new List<OrderItem>()
        };
        return order;
    }

    public void MarkAsProcessed()
    {
        Status = OrderStatus.Processed;
        ProcessedAt = DateTime.UtcNow;
    }
}

public enum OrderStatus
{
    Pending = 0,
    Processed = 1,
    Failed = 2
}

public class OrderItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderId { get; set; }                    // Foreign Key
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public static OrderItem Create(string productName, int quantity, decimal unitPrice)
    {
        return new OrderItem
        {
            ProductName = productName,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
    }
}
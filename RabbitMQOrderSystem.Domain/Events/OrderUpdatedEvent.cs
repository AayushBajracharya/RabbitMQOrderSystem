namespace RabbitMQOrderSystem.Domain.Events
{
    public record OrderUpdatedEvent
    {
        public Guid OrderId { get; init; }
        public string OrderNumber { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    }
}

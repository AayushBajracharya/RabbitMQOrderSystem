namespace RabbitMQOrderSystem.Api.DTOs
{
    public class CreateOrderRequest
    {
        public decimal Amount { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public List<OrderItemRequest> Items { get; set; } = new();
    }
}

using MassTransit;
using RabbitMQOrderSystem.Application.Interfaces;
using RabbitMQOrderSystem.Domain.Events;

namespace RabbitMQOrderSystem.Infrastructure.Publishers
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishOrderCreatedAsync(OrderCreatedEvent orderEvent)
        {
            await _publishEndpoint.Publish(orderEvent);
            Console.WriteLine($"[Publisher] OrderCreatedEvent published: {orderEvent.OrderId}");
        }

        public async Task PublishOrderUpdatedAsync(OrderUpdatedEvent orderEvent)
        {
            await _publishEndpoint.Publish(orderEvent);
            Console.WriteLine($"[Publisher] OrderUpdatedEvent published: {orderEvent.OrderId}");
        }
    }
}

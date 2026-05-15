using RabbitMQOrderSystem.Domain.Events;

namespace RabbitMQOrderSystem.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishOrderCreatedAsync(OrderCreatedEvent orderEvent);
        Task PublishOrderUpdatedAsync(OrderUpdatedEvent orderEvent); 
    }
}

using RabbitMQOrderSystem.Domain.Entities;

namespace RabbitMQOrderSystem.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order> AddAsync(Order order);
    Task<Order?> GetByIdAsync(Guid id);
    Task UpdateAsync(Order order);
}
using POS.Domain.Entities;

namespace POS.Infrastructure.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Order? GetById(int id);
        IEnumerable<Order> GetActiveOrders();
        void Add(Order order);
    }
}

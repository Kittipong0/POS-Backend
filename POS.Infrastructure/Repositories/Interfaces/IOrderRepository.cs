using POS.Domain.Entities;

namespace POS.Infrastructure.Repositories.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        IEnumerable<Order> GetActiveOrders();
        IEnumerable<Order> GetByTableId(int tableId);
        OrderItem? GetOrderItemById(int id);
        void UpdateOrderItem(OrderItem item);
    }
}

using POS.Domain.Entities;
using POS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using POS.Infrastructure.Repositories.Interfaces;

namespace POS.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _writeContext;
        private readonly AppReadDbContext _readContext;

        public OrderRepository(AppDbContext writeContext, AppReadDbContext readContext)
        {
            _writeContext = writeContext;
            _readContext = readContext;
        }

        public Order? GetById(int id)
        {
            return _readContext.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<Order> GetActiveOrders()
        {
            return _readContext.Orders
                .Include(o => o.Items)
                .Where(o => o.Status == Domain.Enums.OrderStatus.Pending || o.Status == Domain.Enums.OrderStatus.Cooking)
                .ToList();
        }

        public void Add(Order order)
        {
            _writeContext.Orders.Add(order);
        }
    }
}

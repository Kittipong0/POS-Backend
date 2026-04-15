using POS.Domain.Entities;
using POS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using POS.Infrastructure.Repositories.Interfaces;

namespace POS.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext writeContext, AppReadDbContext readContext)
            : base(writeContext, readContext)
        {
        }

        public override IEnumerable<Order> GetAll()
        {
            return _readContext.Orders
                .Include(o => o.Items)
                .AsNoTracking()
                .ToList();
        }

        public override Order? GetById(int id)
        {
            return _writeContext.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<Order> GetActiveOrders()
        {
            return _readContext.Orders
                .Include(o => o.Items)
                .AsNoTracking()
                .Where(o => o.Status == Domain.Enums.OrderStatus.Pending
                         || o.Status == Domain.Enums.OrderStatus.Preparing
                         || o.Status == Domain.Enums.OrderStatus.Cooking)
                .ToList();
        }

        public IEnumerable<Order> GetByTableId(int tableId)
        {
            return _writeContext.Orders
                .Include(o => o.Items)
                .Where(o => o.TableId == tableId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public OrderItem? GetOrderItemById(int id)
        {
            return _readContext.OrderItems
                .AsNoTracking()
                .FirstOrDefault(oi => oi.Id == id);
        }

        public void UpdateOrderItem(OrderItem item)
        {
            var existing = _writeContext.OrderItems.Local.FirstOrDefault(e => e.Id == item.Id);
            if (existing != null)
            {
                _writeContext.Entry(existing).CurrentValues.SetValues(item);
            }
            else
            {
                _writeContext.Entry(item).State = EntityState.Modified;
            }
        }
    }
}

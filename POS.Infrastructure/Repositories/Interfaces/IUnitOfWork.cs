using POS.Infrastructure.Repositories.Interfaces;

namespace POS.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderRepository Orders { get; }
        IMenuItemRepository MenuItems { get; }
        ITableRepository Tables { get; }
        IInventoryRepository Inventories { get; }
        int Complete();
    }
}

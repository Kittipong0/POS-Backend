using POS.Infrastructure.Repositories.Interfaces;

namespace POS.Infrastructure.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderRepository Orders { get; }
        IMenuItemRepository MenuItems { get; }
        int Complete();
    }
}

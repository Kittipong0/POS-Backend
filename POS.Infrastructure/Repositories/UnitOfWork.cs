using POS.Infrastructure.Data;
using POS.Infrastructure.Repositories.Interfaces;

namespace POS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly AppReadDbContext _readContext;
    public IOrderRepository Orders { get; private set; }
    public IMenuItemRepository MenuItems { get; private set; }
    public ITableRepository Tables { get; private set; }
    public IInventoryRepository Inventories { get; private set; }

    public UnitOfWork(AppDbContext context, AppReadDbContext readContext)
    {
        _context = context;
        _readContext = readContext;
        Orders = new OrderRepository(_context, _readContext);
        MenuItems = new MenuItemRepository(_context, _readContext);
        Tables = new TableRepository(_context, _readContext);
        Inventories = new InventoryRepository(_context, _readContext);
    }

    public int Complete()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
        _readContext.Dispose();
    }
}



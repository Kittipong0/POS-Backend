using POS.Domain.Entities;
using POS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace POS.Infrastructure.Repositories;

public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }
    int Complete();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IOrderRepository Orders { get; private set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Orders = new OrderRepository(_context);
    }

    public int Complete()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

public interface IOrderRepository
{
    Order? GetById(int id);
    void Add(Order order);
}

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public Order? GetById(int id)
    {
        return _context.Orders
            .Include(o => o.Items)
            .FirstOrDefault(o => o.Id == id);
    }

    public void Add(Order order)
    {
        _context.Orders.Add(order);
    }
}


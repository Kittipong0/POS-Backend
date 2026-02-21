using POS.Application.Models;
using POS.Infrastructure.Repositories;

namespace POS.Application.Services;

public interface IOrderService
{
    Task<OrderDto> CreateOrder(CreateOrderRequest request);
    Task<IEnumerable<OrderDto>> GetActiveOrders();
}

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderDto> CreateOrder(CreateOrderRequest request)
    {
        // Business Logic: Validate stock, create order, deduct inventory
        return new OrderDto();
    }

    public async Task<IEnumerable<OrderDto>> GetActiveOrders()
    {
        return new List<OrderDto>();
    }
}

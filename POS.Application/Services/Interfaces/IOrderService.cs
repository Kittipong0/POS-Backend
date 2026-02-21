using POS.Application.Models;

namespace POS.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrder(CreateOrderRequest request);
        Task<IEnumerable<OrderDto>> GetActiveOrders();
    }
}

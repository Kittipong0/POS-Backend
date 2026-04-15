using POS.Application.Models.Orders;

namespace POS.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrder(CreateOrderRequest request);
        Task<IEnumerable<OrderDto>> GetActiveOrders();
    }
}

using POS.Application.Models.Orders;
using POS.Application.Models.Tables;

namespace POS.Application.Services.Interfaces
{
    public interface IStaffService
    {
        Task<IEnumerable<TableStatusDto>> GetAllTablesStatus();
        Task<OrderDto> PlaceOrderForTable(CreateOrderRequest request);
        Task<IEnumerable<OrderDto>> GetTableOrders(int tableId);
        Task<OrderDto> GetOrderStatus(int orderId);
        Task<CheckoutSummaryDto> GetCheckoutSummary(int tableId);
        Task CheckoutTable(int tableId);
        Task<string> GenerateQrToken(int tableId);
        Task<bool> MarkItemServed(int orderItemId);
        Task<bool> ServeAllReadyItems(int tableId);
    }
}

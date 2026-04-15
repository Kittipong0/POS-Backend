using POS.Application.Models.Menu;
using POS.Application.Models.Orders;
using POS.Application.Models.Tables;
using POS.Application.Models.Common;

namespace POS.Application.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<MenuItemDto>> GetMenu();
        Task<PagedResult<MenuItemDto>> GetMenuPaged(MenuQueryRequest query);
        Task<OrderDto> PlaceOrder(CreateOrderRequest request);
        Task<OrderDto?> GetOrderStatus(int orderId);
        Task<IEnumerable<OrderDto>> GetOrdersByTable(int tableId);
        Task<TableInfoDto> GetTableInfo(int tableId, string qrToken);
    }
}

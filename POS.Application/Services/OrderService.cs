using POS.Application.Models;
using POS.Domain.Entities;
using POS.Domain.Enums;
using POS.Infrastructure.Repositories.Interfaces;
using POS.Application.Services.Interfaces;

namespace POS.Application.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderDto> CreateOrder(CreateOrderRequest request)
    {
        var menuItemIds = request.Items.Select(i => i.MenuItemId).ToList();
        var menuItems = _unitOfWork.MenuItems.GetListByIds(menuItemIds).ToDictionary(m => m.Id);

        var order = new Order
        {
            TableId = request.TableId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            Items = request.Items.Select(itemRequest => 
            {
                if (!menuItems.TryGetValue(itemRequest.MenuItemId, out var menuItem))
                {
                    throw new Exception($"Menu item {itemRequest.MenuItemId} not found");
                }
                return new OrderItem
                {
                    MenuItemId = itemRequest.MenuItemId,
                    Quantity = itemRequest.Quantity,
                    UnitPrice = menuItem.Price
                };
            }).ToList()
        };

        order.TotalAmount = order.Items.Sum(i => i.Quantity * i.UnitPrice);

        _unitOfWork.Orders.Add(order);
        _unitOfWork.Complete();

        return MapToDto(order, menuItems);
    }

    public async Task<IEnumerable<OrderDto>> GetActiveOrders()
    {
        var orders = _unitOfWork.Orders.GetActiveOrders();
        var menuItemIds = orders.SelectMany(o => o.Items).Select(i => i.MenuItemId).Distinct().ToList();
        var menuItems = _unitOfWork.MenuItems.GetListByIds(menuItemIds).ToDictionary(m => m.Id);

        return orders.Select(o => MapToDto(o, menuItems));
    }

    private OrderDto MapToDto(Order order, Dictionary<int, MenuItem> menuItems)
    {
        return new OrderDto
        {
            Id = order.Id,
            TableNumber = $"Table {order.TableId}", // Simplification since we don't have table name easily here
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            Items = order.Items.Select(i => new OrderItemDto
            {
                MenuItemId = i.MenuItemId,
                MenuItemName = menuItems.TryGetValue(i.MenuItemId, out var m) ? m.Name : "Unknown",
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
    }
}

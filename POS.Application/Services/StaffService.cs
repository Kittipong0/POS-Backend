using POS.Application.Models.Menu;
using POS.Application.Models.Orders;
using POS.Application.Models.Inventory;
using POS.Application.Models.Tables;
using POS.Application.Models.Common;
using POS.Application.Services.Interfaces;
using POS.Domain.Entities;
using POS.Domain.Enums;
using POS.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;
using POS.Infrastructure.SignalR;

namespace POS.Application.Services;

public class StaffService : IStaffService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<OrderHub> _hubContext;
    private const int WaitTimeAlertMinutes = 15;

    public StaffService(IUnitOfWork unitOfWork, IHubContext<OrderHub> hubContext)
    {
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
    }

    /// <summary>
    /// Returns all tables with their status plus an alert if wait time > 15 minutes.
    /// </summary>
    public async Task<IEnumerable<TableStatusDto>> GetAllTablesStatus()
    {
        var tables = _unitOfWork.Tables.GetAll().ToList();
        var now = DateTime.UtcNow;
        var orders = _unitOfWork.Orders.GetAll().Where(o => !o.IsPaid && o.Status != OrderStatus.Cancelled).ToList();

        return tables.Select(t =>
        {
            double waitMinutes = 0;
            bool isAlert = false;
            bool hasReadyItems = false;

            if (t.IsOccupied && t.LastOrderTime.HasValue)
            {
                waitMinutes = (now - t.LastOrderTime.Value).TotalMinutes;
                isAlert = waitMinutes >= WaitTimeAlertMinutes;

                // Check if this table has any items in 'ReadyToServe' status
                hasReadyItems = orders
                    .Where(o => o.TableId == t.Id && o.QrToken == t.QrToken)
                    .SelectMany(o => o.Items)
                    .Any(i => i.Status == OrderStatus.ReadyToServe);
            }

            return new TableStatusDto
            {
                Id = t.Id,
                TableNumber = t.TableNumber,
                IsOccupied = t.IsOccupied,
                // Explicitly mark as UTC so the serializer appends 'Z' → frontend parses as UTC correctly
                LastOrderTime = t.LastOrderTime.HasValue
                    ? DateTime.SpecifyKind(t.LastOrderTime.Value, DateTimeKind.Utc)
                    : (DateTime?)null,
                IsWaitTimeAlert = isAlert,
                WaitMinutes = Math.Round(waitMinutes, 1),
                QrToken = t.QrToken,
                HasReadyItems = hasReadyItems
            };
        })
        .OrderByDescending(t => t.Id)
        .ThenByDescending(t => t.WaitMinutes);
    }

    /// <summary>
    /// Staff places an order on behalf of a table (seamless handover).
    /// The order is automatically routed to Kitchen/Bar via the order items.
    /// </summary>
    public async Task<OrderDto> PlaceOrderForTable(CreateOrderRequest request)
    {
        var menuItemIds = request.Items.Select(i => i.MenuItemId).ToList();
        var menuItems = _unitOfWork.MenuItems.GetListByIds(menuItemIds).ToDictionary(m => m.Id);
        var table = _unitOfWork.Tables.GetById(request.TableId);

        var order = new Order
        {
            TableId = request.TableId,
            QrToken = table?.QrToken,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            Items = request.Items.Select(itemRequest =>
            {
                if (!menuItems.TryGetValue(itemRequest.MenuItemId, out var menuItem))
                    throw new Exception($"Menu item {itemRequest.MenuItemId} not found");

                return new OrderItem
                {
                    MenuItemId = itemRequest.MenuItemId,
                    Quantity = itemRequest.Quantity,
                    UnitPrice = menuItem.Price,
                    Status = OrderStatus.Pending
                };
            }).ToList()
        };

        decimal subTotal = order.Items.Sum(i => i.Quantity * i.UnitPrice);
        order.ServiceCharge = Math.Round(subTotal * 0.10m, 2);
        order.Vat = Math.Round((subTotal + order.ServiceCharge) * 0.07m, 2);
        order.TotalAmount = subTotal + order.ServiceCharge + order.Vat;

        // Update table status
        if (table != null)
        {
            table.IsOccupied = true;
            table.LastOrderTime = DateTime.UtcNow;
            _unitOfWork.Tables.Update(table);
        }

        _unitOfWork.Orders.Add(order);
        _unitOfWork.Complete();

        // Broadcast: Alert Kitchen and Staff
        await _hubContext.Clients.All.SendAsync("ReceiveOrderStatus", new { orderId = order.Id, status = order.Status.ToString() });
        if (table != null)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveTableStatus", new { tableId = table.Id, isOccupied = table.IsOccupied });
        }

        return new OrderDto
        {
            Id = order.Id,
            TableNumber = table?.TableNumber ?? $"Table {order.TableId}",
            TotalAmount = order.TotalAmount,
            ServiceCharge = order.ServiceCharge,
            Vat = order.Vat,
            Status = order.Status,
            QrToken = order.QrToken,
            Items = order.Items.Select(i => new OrderItemDto
            {
                MenuItemId = i.MenuItemId,
                MenuItemName = menuItems.TryGetValue(i.MenuItemId, out var m) ? m.Name : "Unknown",
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Status = i.Status
            }).ToList()
        };
    }

    public async Task<IEnumerable<OrderDto>> GetTableOrders(int tableId)
    {
        var table = _unitOfWork.Tables.GetById(tableId);

        // Guard: if there's no active QR session, return empty list
        if (table == null || string.IsNullOrEmpty(table.QrToken))
            return Enumerable.Empty<OrderDto>();

        // Only return unpaid, non-cancelled orders from the current QR session
        var orders = _unitOfWork.Orders.GetByTableId(tableId)
            .Where(o => !o.IsPaid
                     && o.Status != OrderStatus.Cancelled
                     && o.QrToken == table.QrToken)
            .ToList();

        var menuItemIds = orders.SelectMany(o => o.Items).Select(i => i.MenuItemId).Distinct().ToList();
        var menuItems = _unitOfWork.MenuItems.GetListByIds(menuItemIds).ToDictionary(m => m.Id);

        return orders.Select(o => new OrderDto
        {
            Id = o.Id,
            TableNumber = table?.TableNumber ?? $"Table {tableId}",
            TotalAmount = o.TotalAmount,
            ServiceCharge = o.ServiceCharge,
            Vat = o.Vat,
            Status = o.Status,
            QrToken = o.QrToken,
            Items = o.Items.Select(i => new OrderItemDto
            {
                MenuItemId = i.MenuItemId,
                MenuItemName = menuItems.TryGetValue(i.MenuItemId, out var m) ? m.Name : "Unknown",
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Status = i.Status
            }).ToList()
        }).ToList();
    }

    public async Task<OrderDto> GetOrderStatus(int orderId)
    {
        var order = _unitOfWork.Orders.GetById(orderId);
        if (order == null) return null;

        var menuItemIds = order.Items.Select(i => i.MenuItemId).ToList();
        var menuItems = _unitOfWork.MenuItems.GetListByIds(menuItemIds).ToDictionary(m => m.Id);
        var table = _unitOfWork.Tables.GetById(order.TableId);

        return new OrderDto
        {
            Id = order.Id,
            TableNumber = table?.TableNumber ?? $"Table {order.TableId}",
            TotalAmount = order.TotalAmount,
            ServiceCharge = order.ServiceCharge,
            Vat = order.Vat,
            Status = order.Status,
            QrToken = order.QrToken,
            Items = order.Items.Select(i => new OrderItemDto
            {
                MenuItemId = i.MenuItemId,
                MenuItemName = menuItems.TryGetValue(i.MenuItemId, out var m) ? m.Name : "Unknown",
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Status = i.Status
            }).ToList()
        };
    }
    public async Task<CheckoutSummaryDto> GetCheckoutSummary(int tableId)
    {
        var table = _unitOfWork.Tables.GetById(tableId);

        // Only bill unpaid orders from the current QR session
        var orders = _unitOfWork.Orders.GetByTableId(tableId)
            .Where(o => !o.IsPaid
                     && o.Status != OrderStatus.Cancelled
                     && !string.IsNullOrEmpty(table.QrToken)
                     && o.QrToken == table.QrToken)
            .ToList();

        var menuItemIds = orders.SelectMany(o => o.Items).Select(i => i.MenuItemId).Distinct().ToList();
        var menuItems = _unitOfWork.MenuItems.GetListByIds(menuItemIds).ToDictionary(m => m.Id);

        var activeOrderDtos = orders.Select(o => new OrderDto
        {
            Id = o.Id,
            TableNumber = table?.TableNumber ?? $"Table {tableId}",
            TotalAmount = o.TotalAmount,
            ServiceCharge = o.ServiceCharge,
            Vat = o.Vat,
            Status = o.Status,
            QrToken = o.QrToken,
            Items = o.Items.Select(i => new OrderItemDto
            {
                MenuItemId = i.MenuItemId,
                MenuItemName = menuItems.TryGetValue(i.MenuItemId, out var m) ? m.Name : "Unknown",
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Status = i.Status
            }).ToList()
        }).ToList();

        decimal serviceCharge = orders.Sum(o => o.ServiceCharge);
        decimal vat = orders.Sum(o => o.Vat);
        decimal totalAmount = orders.Sum(o => o.TotalAmount) - serviceCharge - vat;

        return new CheckoutSummaryDto
        {
            TableId = tableId,
            TableNumber = table?.TableNumber ?? $"Table {tableId}",
            Orders = activeOrderDtos,
            TotalAmount = totalAmount,
            ServiceCharge = serviceCharge,
            Vat = vat,
            GrandTotal = totalAmount + serviceCharge + vat,
            BillDate = DateTime.UtcNow
        };
    }

    public async Task CheckoutTable(int tableId)
    {
        var orders = _unitOfWork.Orders.GetByTableId(tableId)
            .Where(o => !o.IsPaid && o.Status != OrderStatus.Cancelled)
            .ToList();

        foreach (var order in orders)
        {
            order.Status = OrderStatus.Completed;
            order.IsPaid = true;
            foreach (var item in order.Items)
            {
                item.Status = OrderStatus.Completed;
            }
        }

        var table = _unitOfWork.Tables.GetById(tableId);
        if (table != null)
        {
            table.IsOccupied = false;
            table.LastOrderTime = null;
            table.QrToken = null; // Clear token on checkout
            _unitOfWork.Tables.Update(table);
        }

        _unitOfWork.Complete();

        // Broadcast: Table is now vacant
        if (table != null)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveTableStatus", new { tableId = table.Id, isOccupied = false });
        }
    }

    public async Task<string> GenerateQrToken(int tableId)
    {
        var table = _unitOfWork.Tables.GetById(tableId);
        if (table == null) throw new Exception("Table not found");

        var newToken = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        table.QrToken = newToken;

        _unitOfWork.Tables.Update(table);
        _unitOfWork.Complete();

        // Broadcast: QR Token generated (Staff screen might want to refresh)
        await _hubContext.Clients.All.SendAsync("ReceiveTableStatus", new { tableId = table.Id, qrToken = newToken });

        return newToken;
    }
    public async Task<bool> MarkItemServed(int orderItemId)
    {
        var item = _unitOfWork.Orders.GetOrderItemById(orderItemId);
        if (item == null) return false;

        item.Status = OrderStatus.Served;
        _unitOfWork.Orders.UpdateOrderItem(item);

        // Check if all items in parent order are served
        var order = _unitOfWork.Orders.GetById(item.OrderId);
        if (order != null && order.Items.All(i => i.Status == OrderStatus.Served || i.Status == OrderStatus.Completed))
        {
            order.Status = OrderStatus.Served;
            _unitOfWork.Orders.Update(order);
        }

        _unitOfWork.Complete();

        // Notify client and staff
        await _hubContext.Clients.All.SendAsync("ReceiveOrderStatus", 
            new { orderId = item.OrderId, itemId = orderItemId, status = (int)OrderStatus.Served });

        return true;
    }

    public async Task<bool> ServeAllReadyItems(int tableId)
    {
        var table = _unitOfWork.Tables.GetById(tableId);
        if (table == null || string.IsNullOrEmpty(table.QrToken)) return false;

        var orders = _unitOfWork.Orders.GetByTableId(tableId)
            .Where(o => !o.IsPaid && o.Status != OrderStatus.Cancelled && o.QrToken == table.QrToken)
            .ToList();

        var readyItems = orders.SelectMany(o => o.Items)
            .Where(i => i.Status == OrderStatus.ReadyToServe)
            .ToList();

        if (!readyItems.Any()) return false;

        foreach (var item in readyItems)
        {
            item.Status = OrderStatus.Served;
            _unitOfWork.Orders.UpdateOrderItem(item);
        }

        // Update parent orders if all items are now served
        foreach (var order in orders)
        {
            if (order.Items.All(i => i.Status == OrderStatus.Served || i.Status == OrderStatus.Completed))
            {
                order.Status = OrderStatus.Served;
                _unitOfWork.Orders.Update(order);
            }
        }

        _unitOfWork.Complete();

        // Broadcast updates for each affected order
        var affectedOrderIds = readyItems.Select(i => i.OrderId).Distinct();
        foreach (var orderId in affectedOrderIds)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveOrderStatus", 
                new { orderId = orderId, status = (int)OrderStatus.Served });
        }
        
        await _hubContext.Clients.All.SendAsync("ReceiveTableStatus", new { tableId = tableId });

        return true;
    }
}

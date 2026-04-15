using POS.Application.Models.Menu;
using POS.Application.Models.Orders;
using POS.Application.Models.Tables;
using POS.Application.Models.Common;
using POS.Application.Services.Interfaces;
using POS.Domain.Entities;
using POS.Domain.Enums;
using POS.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;
using POS.Infrastructure.SignalR;

namespace POS.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<OrderHub> _hubContext;

    private static readonly Dictionary<int, string> CategoryNames = new()
    {
        { 1, "อาหารจานหลัก" },
        { 2, "เครื่องดื่ม" },
        { 3, "ของหวาน" },
        { 4, "เมนูพิเศษ" }
    };

    private string GetCategoryName(int id) => CategoryNames.TryGetValue(id, out var name) ? name : $"ประเภท {id}";

    public CustomerService(IUnitOfWork unitOfWork, IHubContext<OrderHub> hubContext)
    {
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
    }

    /// <summary>
    /// Returns the full menu with recommended items highlighted.
    /// Recommended = IsRecommended flag OR high profit margin OR high sales count.
    /// </summary>
    public async Task<IEnumerable<MenuItemDto>> GetMenu()
    {
        var items = _unitOfWork.MenuItems.GetAll();

        return items
            .Where(m => m.IsAvailable)
            .OrderByDescending(m => m.IsRecommended)
            .ThenByDescending(m => m.SalesCount)
            .Select(m => new MenuItemDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                IsAvailable = m.IsAvailable,
                IsRecommended = m.IsRecommended || m.SalesCount > 50 || (m.Price - m.Cost) > m.Cost,
                CategoryId = m.CategoryId,
                Category = m.Category == null ? GetCategoryName(m.CategoryId) : m.Category.Name,
                NameEn = m.NameEn,
                Image = m.Image
            });
    }

    /// <summary>
    /// Returns paginated, searchable, and filterable menu items.
    /// </summary>
    public async Task<PagedResult<MenuItemDto>> GetMenuPaged(MenuQueryRequest query)
    {
        var baseQuery = _unitOfWork.MenuItems.QueryMenu(
            query.SearchKeyword,
            query.CategoryId,
            query.MinPrice,
            query.MaxPrice,
            query.OnlyRecommended);

        // Get all available categories for filter chips — use real names
        var allCategories = _unitOfWork.MenuItems.GetAll()
            .Where(m => m.IsAvailable)
            .GroupBy(m => m.CategoryId)
            .Select(g => new CategoryDto 
            { 
                Id = g.Key, 
                Name = GetCategoryName(g.Key) 
            })
            .OrderBy(c => c.Name)
            .ToList();

        // Apply sorting
        var sorted = query.SortBy.ToLower() switch
        {
            "name" => baseQuery.OrderBy(m => m.Name),
            "price" => baseQuery.OrderBy(m => m.Price),
            "price_desc" => baseQuery.OrderByDescending(m => m.Price),
            _ => baseQuery.OrderByDescending(m => m.IsRecommended).ThenByDescending(m => m.SalesCount)
        };

        var totalCount = sorted.Count();

        var items = sorted
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList() // Materialize first to allow custom C# logic in Select
            .Select(m => new MenuItemDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                IsAvailable = m.IsAvailable,
                IsRecommended = m.IsRecommended || m.SalesCount > 50 || (m.Price - m.Cost) > m.Cost,
                CategoryId = m.CategoryId,
                Category = m.Category?.Name ?? GetCategoryName(m.CategoryId),
                NameEn = m.NameEn,
                Image = m.Image
            })
            .ToList();

        return new PagedResult<MenuItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize,
            AvailableCategories = allCategories
        };
    }

    /// <summary>
    /// Places a new order from the customer QR scan flow.
    /// Updates the table's LastOrderTime for staff monitoring.
    /// </summary>
    public async Task<OrderDto> PlaceOrder(CreateOrderRequest request)
    {
        var menuItemIds = request.Items.Select(i => i.MenuItemId).ToList();
        var menuItems = _unitOfWork.MenuItems.GetListByIds(menuItemIds).ToDictionary(m => m.Id);

        var table = _unitOfWork.Tables.GetById(request.TableId);
        if (table == null) throw new Exception("Table not found");

        // Validate QR Token
        if (string.IsNullOrEmpty(request.QrToken) || table.QrToken != request.QrToken)
        {
            throw new Exception("Invalid or expired QR Code session. Please ask staff for a new QR Code.");
        }

        var order = new Order
        {
            TableId = request.TableId,
            QrToken = request.QrToken,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            IsPaid = false,
            Items = request.Items.Select(itemRequest =>
            {
                if (!menuItems.TryGetValue(itemRequest.MenuItemId, out var menuItem))
                    throw new Exception($"Menu item {itemRequest.MenuItemId} not found");
                if (!menuItem.IsAvailable)
                    throw new Exception($"Menu item '{menuItem.Name}' is currently unavailable");

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

        // Update table's last order time for wait-time monitoring
        table.IsOccupied = true;
        table.LastOrderTime = DateTime.UtcNow;
        _unitOfWork.Tables.Update(table);

        // Update sales count
        foreach (var item in order.Items)
        {
            if (menuItems.TryGetValue(item.MenuItemId, out var mi))
            {
                mi.SalesCount += item.Quantity;
                _unitOfWork.MenuItems.Update(mi);
            }
        }

        _unitOfWork.Orders.Add(order);
        _unitOfWork.Complete();

        // Broadcast: Alert Kitchen and Staff
        await _hubContext.Clients.All.SendAsync("ReceiveOrderStatus", new { orderId = order.Id, status = order.Status.ToString() });
        await _hubContext.Clients.All.SendAsync("ReceiveTableStatus", new { tableId = table.Id, isOccupied = table.IsOccupied });

        return new OrderDto
        {
            Id = order.Id,
            TableNumber = table.TableNumber,
            TotalAmount = order.TotalAmount,
            ServiceCharge = order.ServiceCharge,
            Vat = order.Vat,
            Status = order.Status,
            QrToken = order.QrToken,
            EstimatedReadyTime = null,
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

    /// <summary>
    /// Real-time order status tracking for the customer.
    /// </summary>
    public async Task<OrderDto?> GetOrderStatus(int orderId)
    {
        var order = _unitOfWork.Orders.GetById(orderId);
        if (order == null) return null;

        var menuItemIds = order.Items.Select(i => i.MenuItemId).Distinct().ToList();
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
            EstimatedReadyTime = order.EstimatedReadyTime,
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
    /// <summary>
    /// Retrieves all orders for a specific table.
    /// Useful for customers to track multiple orders (e.g., re-orders).
    /// </summary>
    public async Task<IEnumerable<OrderDto>> GetOrdersByTable(int tableId)
    {
        var table = _unitOfWork.Tables.GetById(tableId);
        var orders = _unitOfWork.Orders.GetByTableId(tableId)
            .Where(o => !o.IsPaid && o.QrToken == table?.QrToken); // Only show active session orders

        // Batch fetch all menu items involved to avoid N+1 queries during mapping
        var menuItemIds = orders.SelectMany(o => o.Items).Select(i => i.MenuItemId).Distinct().ToList();
        var menuItems = _unitOfWork.MenuItems.GetListByIds(menuItemIds).ToDictionary(m => m.Id);

        return orders.Select(order => new OrderDto
        {
            Id = order.Id,
            TableNumber = table?.TableNumber ?? $"Table {order.TableId}",
            TotalAmount = order.TotalAmount,
            ServiceCharge = order.ServiceCharge,
            Vat = order.Vat,
            Status = order.Status,
            QrToken = order.QrToken,
            EstimatedReadyTime = order.EstimatedReadyTime,
            Items = order.Items.Select(i => new OrderItemDto
            {
                MenuItemId = i.MenuItemId,
                MenuItemName = menuItems.TryGetValue(i.MenuItemId, out var m) ? m.Name : "Unknown",
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Status = i.Status
            }).ToList()
        });
    }

    public async Task<TableInfoDto> GetTableInfo(int tableId, string qrToken)
    {
        var table = _unitOfWork.Tables.GetById(tableId);
        if (table == null) throw new Exception("Table not found");

        // Validate QR Token
        if (string.IsNullOrEmpty(qrToken) || table.QrToken != qrToken)
        {
            throw new Exception("Invalid or expired QR Code session. Please ask staff for a new QR Code.");
        }

        return new TableInfoDto
        {
            Id = table.Id,
            TableNumber = table.TableNumber,
            IsOccupied = table.IsOccupied
        };
    }
}

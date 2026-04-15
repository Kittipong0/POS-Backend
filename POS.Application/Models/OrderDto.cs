using POS.Domain.Enums;

namespace POS.Application.Models;

public class OrderDto
{
    public int Id { get; set; }
    public string TableNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal ServiceCharge { get; set; }
    public decimal Vat { get; set; }
    public OrderStatus Status { get; set; }
    public string? QrToken { get; set; }
    public DateTime? EstimatedReadyTime { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public int MenuItemId { get; set; }
    public string MenuItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public OrderStatus Status { get; set; }
}

public class CreateOrderRequest
{
    public int TableId { get; set; }
    public string? QrToken { get; set; }
    public List<CreateOrderItemRequest> Items { get; set; } = new();
}

public class CreateOrderItemRequest
{
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }
}

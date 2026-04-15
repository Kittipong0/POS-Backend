using POS.Domain.Enums;

namespace POS.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int TableId { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal ServiceCharge { get; set; }
    public decimal Vat { get; set; }
    public bool IsPaid { get; set; }
    public string? QrToken { get; set; }
    public DateTime? EstimatedReadyTime { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public OrderStatus Status { get; set; }
}

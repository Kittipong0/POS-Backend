using POS.Domain.Enums;

namespace POS.Application.Models.Orders;

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

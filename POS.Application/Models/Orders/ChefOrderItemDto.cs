namespace POS.Application.Models.Orders;

public class ChefOrderItemDto
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public string TableNumber { get; set; } = string.Empty;
    public string MenuItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
}

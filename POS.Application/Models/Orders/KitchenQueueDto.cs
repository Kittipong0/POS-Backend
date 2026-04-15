namespace POS.Application.Models.Orders;

public class KitchenQueueDto
{
    public string MenuItemName { get; set; } = string.Empty;
    public int TotalQuantity { get; set; }
    public List<int> OrderItemIds { get; set; } = new();
    public string Status { get; set; } = string.Empty;
}

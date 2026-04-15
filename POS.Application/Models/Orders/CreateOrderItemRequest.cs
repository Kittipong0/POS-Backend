namespace POS.Application.Models.Orders;

public class CreateOrderItemRequest
{
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }
}

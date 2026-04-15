namespace POS.Application.Models.Orders;

public class CreateOrderRequest
{
    public int TableId { get; set; }
    public string? QrToken { get; set; }
    public List<CreateOrderItemRequest> Items { get; set; } = new();
}

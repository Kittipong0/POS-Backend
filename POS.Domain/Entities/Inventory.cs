namespace POS.Domain.Entities;

public class Inventory
{
    public int Id { get; set; }
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
}

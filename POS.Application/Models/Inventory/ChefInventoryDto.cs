namespace POS.Application.Models.Inventory;

public class ChefInventoryDto
{
    public int MenuItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public bool IsAvailable { get; set; }
    public string? Image { get; set; }
}

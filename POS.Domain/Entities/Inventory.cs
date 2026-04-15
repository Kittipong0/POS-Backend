namespace POS.Domain.Entities;

public class Inventory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int Quantity { get; set; }
    public int MinimumQuantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal CostPerUnit { get; set; }
    public string? Supplier { get; set; }
    public DateTime LastUpdated { get; set; }

    // Navigation
    public InventoryCategory? Category { get; set; }
    public ICollection<MenuItemIngredient> MenuItemIngredients { get; set; } = new List<MenuItemIngredient>();
}

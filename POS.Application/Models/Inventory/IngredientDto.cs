namespace POS.Application.Models.Inventory;

public class IngredientDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal CostPerUnit { get; set; }
    public string? Supplier { get; set; }
}

namespace POS.Application.Models.Menu;

public class IngredientRequest
{
    public int InventoryId { get; set; }
    public decimal QuantityUsed { get; set; }
    public string Unit { get; set; } = string.Empty;
}

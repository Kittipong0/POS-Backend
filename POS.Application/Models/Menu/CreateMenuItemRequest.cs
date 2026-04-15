namespace POS.Application.Models.Menu;

public class CreateMenuItemRequest
{
    public string Name { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int? CategoryId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Image { get; set; }
    public List<IngredientRequest> Ingredients { get; set; } = new();
}

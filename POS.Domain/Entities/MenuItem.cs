namespace POS.Domain.Entities;

public class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsRecommended { get; set; }
    public decimal Cost { get; set; }
    public int SalesCount { get; set; }
    public string? Image { get; set; }

    // Navigation
    public MenuCategory? Category { get; set; }
    public ICollection<MenuItemIngredient> Ingredients { get; set; } = new List<MenuItemIngredient>();
}

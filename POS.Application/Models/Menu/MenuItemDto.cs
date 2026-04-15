namespace POS.Application.Models.Menu;

public class MenuItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameEn { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsRecommended { get; set; }
    public int CategoryId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Image { get; set; }
}

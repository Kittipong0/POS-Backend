namespace POS.Application.Models;

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

public class TableStatusDto
{
    public int Id { get; set; }
    public string TableNumber { get; set; } = string.Empty;
    public bool IsOccupied { get; set; }
    public DateTime? LastOrderTime { get; set; }
    public bool IsWaitTimeAlert { get; set; }
    public double WaitMinutes { get; set; }
    public string? QrToken { get; set; }
    public bool HasReadyItems { get; set; }
}

public class KitchenQueueDto
{
    public string MenuItemName { get; set; } = string.Empty;
    public int TotalQuantity { get; set; }
    public List<int> OrderItemIds { get; set; } = new();
    public string Status { get; set; } = string.Empty;
}

public class ChefOrderItemDto
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public string TableNumber { get; set; } = string.Empty;
    public string MenuItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
}

public class ChefInventoryDto
{
    public int MenuItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public bool IsAvailable { get; set; }
    public string? Image { get; set; }
}

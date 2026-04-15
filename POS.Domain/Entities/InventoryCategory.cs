using System.Collections.Generic;

namespace POS.Domain.Entities;

public class InventoryCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation
    public ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}

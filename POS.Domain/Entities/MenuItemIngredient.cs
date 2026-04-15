namespace POS.Domain.Entities;

public class MenuItemIngredient
{
    public int Id { get; set; }
    public int MenuItemId { get; set; }
    public int InventoryId { get; set; }
    public decimal QuantityUsed { get; set; }  // จำนวนที่ใช้ต่อ 1 จาน
    public string Unit { get; set; } = string.Empty; // หน่วย (กรัม, มล., ชิ้น)

    // Navigation
    public MenuItem? MenuItem { get; set; }
    public Inventory? Inventory { get; set; }
}

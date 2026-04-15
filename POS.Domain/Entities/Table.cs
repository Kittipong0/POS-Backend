namespace POS.Domain.Entities;

public class Table
{
    public int Id { get; set; }
    public string TableNumber { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool IsOccupied { get; set; }
    public string? QrToken { get; set; }
    public DateTime? LastOrderTime { get; set; }
}

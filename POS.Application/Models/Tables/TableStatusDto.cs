namespace POS.Application.Models.Tables;

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

namespace POS.Application.Models.Tables;

public class TableInfoDto
{
    public int Id { get; set; }
    public string TableNumber { get; set; } = string.Empty;
    public bool IsOccupied { get; set; }
}

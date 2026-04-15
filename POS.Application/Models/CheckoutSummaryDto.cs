using POS.Application.Models;

namespace POS.Application.Models;

public class CheckoutSummaryDto
{
    public int TableId { get; set; }
    public string TableNumber { get; set; } = string.Empty;
    public List<OrderDto> Orders { get; set; } = new();
    public decimal ServiceCharge { get; set; }
    public decimal Vat { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal GrandTotal { get; set; }
    public DateTime BillDate { get; set; }
}

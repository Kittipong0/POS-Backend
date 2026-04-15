using Microsoft.AspNetCore.Mvc;
using POS.Application.Models.Orders;
using POS.Application.Models.Tables;
using POS.Application.Services.Interfaces;

namespace POS.API.Controllers;

/// <summary>
/// Staff API: Table monitoring with wait-time alerts, and seamless order handover.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }

    /// <summary>
    /// Get all tables with status, wait time, and alert flags (> 15 minutes).
    /// </summary>
    [HttpGet("tables")]
    public async Task<IActionResult> GetAllTablesStatus()
    {
        var result = await _staffService.GetAllTablesStatus();
        return Ok(result);
    }

    /// <summary>
    /// Staff places an order for a table (mobile/tablet handover).
    /// Orders are automatically routed to kitchen and bar.
    /// </summary>
    [HttpPost("order")]
    public async Task<IActionResult> PlaceOrderForTable([FromBody] CreateOrderRequest request)
    {
        try
        {
            var result = await _staffService.PlaceOrderForTable(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get bill summary for a table before payment.
    /// </summary>
    [HttpGet("table/{tableId}/checkout-summary")]
    public async Task<IActionResult> GetCheckoutSummary(int tableId)
    {
        var result = await _staffService.GetCheckoutSummary(tableId);
        return Ok(result);
    }

    /// <summary>
    /// Get bill summary for a table before payment (Alias for frontend).
    /// </summary>
    [HttpGet("GetBill")]
    public async Task<IActionResult> GetBill([FromQuery] int tableId)
    {
        var result = await _staffService.GetCheckoutSummary(tableId);
        return Ok(result);
    }

    /// <summary>
    /// Finalize payment and release table.
    /// </summary>
    [HttpPost("table/{tableId}/checkout")]
    public async Task<IActionResult> Checkout(int tableId)
    {
        try
        {
            await _staffService.CheckoutTable(tableId);
            return Ok(new { message = $"Table {tableId} checked out successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get all current orders for a specific table.
    /// </summary>
    [HttpGet("Table/{tableId}/Orders")]
    public async Task<IActionResult> GetTableOrders(int tableId)
    {
        var result = await _staffService.GetTableOrders(tableId);
        return Ok(result);
    }

    /// <summary>
    /// Get details and status for a specific order.
    /// </summary>
    [HttpGet("Order/{orderId}/Status")]
    public async Task<IActionResult> GetOrderStatus(int orderId)
    {
        var result = await _staffService.GetOrderStatus(orderId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    /// <summary>
    /// Generate a new QR token for a table.
    /// </summary>
    [HttpPost("table/{tableId}/qr-token")]
    public async Task<IActionResult> GenerateQrToken(int tableId)
    {
        try
        {
            var token = await _staffService.GenerateQrToken(tableId);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("item/{id}/serve")]
    public async Task<IActionResult> MarkItemServed(int id)
    {
        var result = await _staffService.MarkItemServed(id);
        if (!result) return NotFound(new { message = "Order item not found" });
        return Ok(new { message = "Item marked as served" });
    }

    [HttpPost("table/{tableId}/serve-ready")]
    public async Task<IActionResult> ServeAllReadyItems(int tableId)
    {
        try
        {
            var result = await _staffService.ServeAllReadyItems(tableId);
            if (!result) return BadRequest(new { message = "No ready items found to serve" });
            return Ok(new { message = "All ready items marked as served" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

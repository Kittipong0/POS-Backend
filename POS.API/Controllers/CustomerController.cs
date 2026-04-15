using Microsoft.AspNetCore.Mvc;
using POS.Application.Models.Menu;
using POS.Application.Models.Orders;
using POS.Application.Models.Tables;
using POS.Application.Models.Common;
using POS.Application.Services.Interfaces;

namespace POS.API.Controllers;

/// <summary>
/// Customer-facing API: QR ordering, menu recommendations, and real-time order tracking.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// Get all available menu items with recommended items highlighted.
    /// </summary>
    [HttpGet("menu")]
    public async Task<IActionResult> GetMenu()
    {
        var result = await _customerService.GetMenu();
        return Ok(result);
    }

    /// <summary>
    /// Get paginated, searchable, and filterable menu items.
    /// Supports: keyword, categoryId, minPrice, maxPrice, onlyRecommended, page, pageSize, sortBy
    /// </summary>
    [HttpGet("menu/paged")]
    public async Task<IActionResult> GetMenuPaged([FromQuery] MenuQueryRequest query)
    {
        var result = await _customerService.GetMenuPaged(query);
        return Ok(result);
    }

    /// <summary>
    /// Place a new order (via QR code scan).
    /// </summary>
    [HttpPost("order")]
    public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderRequest request)
    {
        try
        {
            var result = await _customerService.PlaceOrder(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Real-time order status: Preparing → Cooking → Ready to Serve.
    /// </summary>
    [HttpGet("order/{id}/status")]
    public async Task<IActionResult> GetOrderStatus(int id)
    {
        var result = await _customerService.GetOrderStatus(id);
        if (result == null) return NotFound(new { message = "Order not found" });
        return Ok(result);
    }

    /// <summary>
    /// Get all orders for a specific table.
    /// </summary>
    [HttpGet("table/{tableId}/orders")]
    public async Task<IActionResult> GetOrdersByTable(int tableId)
    {
        var result = await _customerService.GetOrdersByTable(tableId);
        return Ok(result);
    }

    /// <summary>
    /// Gets Table info and validates the QR token.
    /// </summary>
    [HttpGet("table/{tableId}/info")]
    public async Task<IActionResult> GetTableInfo(int tableId, [FromQuery] string qrToken)
    {
        try
        {
            var result = await _customerService.GetTableInfo(tableId, qrToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

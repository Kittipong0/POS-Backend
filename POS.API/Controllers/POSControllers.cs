using Microsoft.AspNetCore.Mvc;
using POS.Application.Models;
using POS.Application.Services;
using POS.Application.Services.Interfaces;

namespace POS.API.Controllers;

/// <summary>
/// Controller for managing customer-related operations like creating orders.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IOrderService _orderService;

    public CustomerController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Creates a new order for a customer.
    /// </summary>
    /// <param name="request">The order details.</param>
    /// <returns>The result of the order creation.</returns>
    [HttpPost("order")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var result = await _orderService.CreateOrder(request);
        return Ok(result);
    }
}

/// <summary>
/// Controller for managing kitchen-related operations like viewing pending orders.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class KitchenController : ControllerBase
{
    private readonly IOrderService _orderService;

    public KitchenController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Retrieves a list of all pending orders for the kitchen.
    /// </summary>
    /// <returns>A list of pending orders.</returns>
    [HttpGet("pending-orders")]
    public async Task<IActionResult> GetPendingOrders()
    {
        var result = await _orderService.GetActiveOrders();
        return Ok(result);
    }
}

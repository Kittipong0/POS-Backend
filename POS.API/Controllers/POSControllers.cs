using Microsoft.AspNetCore.Mvc;
using POS.Application.Models;
using POS.Application.Services;

namespace POS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IOrderService _orderService;

    public CustomerController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("order")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var result = await _orderService.CreateOrder(request);
        return Ok(result);
    }
}

[ApiController]
[Route("api/[controller]")]
public class KitchenController : ControllerBase
{
    [HttpGet("pending-orders")]
    public IActionResult GetPendingOrders()
    {
        return Ok(new List<OrderDto>());
    }
}

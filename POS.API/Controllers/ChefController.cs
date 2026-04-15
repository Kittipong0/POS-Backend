using Microsoft.AspNetCore.Mvc;
using POS.Application.Models.Menu;
using POS.Application.Models.Orders;
using POS.Application.Models.Inventory;
using POS.Application.Models.Tables;
using POS.Application.Models.Common;
using POS.Application.Services.Interfaces;

namespace POS.API.Controllers;

/// <summary>
/// Chef API: Smart Kitchen Display System (KDS), inventory management, and menu toggle.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ChefController : ControllerBase
{
    private readonly IChefService _chefService;

    public ChefController(IChefService chefService)
    {
        _chefService = chefService;
    }

    [HttpGet("queue")]
    public async Task<IActionResult> GetKitchenQueue()
    {
        var result = await _chefService.GetKitchenQueue();
        return Ok(result);
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetActiveOrders()
    {
        var result = await _chefService.GetActiveOrdersDetailed();
        return Ok(result);
    }

    [HttpGet("inventory")]
    public async Task<IActionResult> GetInventory()
    {
        var result = await _chefService.GetInventoryDetails();
        return Ok(result);
    }

    /// <summary>
    /// Get all raw materials/ingredients for dropdown selection.
    /// </summary>
    [HttpGet("ingredients")]
    public async Task<IActionResult> GetIngredients()
    {
        var result = await _chefService.GetAllIngredients();
        return Ok(result);
    }

    [HttpPatch("item/{id}/cooking")]
    public async Task<IActionResult> MarkItemCooking(int id)
    {
        var result = await _chefService.MarkItemCooking(id);
        if (!result) return NotFound(new { message = "Order item not found" });
        return Ok(new { message = "Item marked as cooking" });
    }

    [HttpPost("items/cooking")]
    public async Task<IActionResult> MarkItemsCooking([FromBody] List<int> ids)
    {
        var result = await _chefService.MarkItemsCooking(ids);
        if (!result) return BadRequest(new { message = "Failed to update items" });
        return Ok(new { message = "Items marked as cooking" });
    }

    [HttpPatch("item/{id}/ready")]
    public async Task<IActionResult> MarkItemReady(int id)
    {
        var result = await _chefService.MarkItemReady(id);
        if (!result) return NotFound(new { message = "Order item not found" });
        return Ok(new { message = "Item marked as ready, inventory updated" });
    }

    [HttpPost("items/ready")]
    public async Task<IActionResult> MarkItemsReady([FromBody] List<int> ids)
    {
        var result = await _chefService.MarkItemsReady(ids);
        if (!result) return BadRequest(new { message = "Failed to update items" });
        return Ok(new { message = "Items marked as ready" });
    }

    [HttpPatch("menu/{id}/toggle")]
    public async Task<IActionResult> ToggleMenuAvailability(int id)
    {
        var result = await _chefService.ToggleMenuAvailability(id);
        if (!result) return NotFound(new { message = "Menu item not found" });
        return Ok(new { message = "Menu availability toggled" });
    }

    /// <summary>
    /// AI Analysis: Send base64 image of a dish, get menu suggestions with ingredients.
    /// </summary>
    [HttpPost("analyze")]
    public async Task<IActionResult> AnalyzeDish([FromBody] AiAnalysisRequest request)
    {
        try
        {
            var result = await _chefService.AnalyzeMenuItem(request.Image, request.Name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("menu")]
    public async Task<IActionResult> CreateMenuItem([FromBody] CreateMenuItemRequest request)
    {
        var success = await _chefService.CreateMenuItem(request);
        if (!success) return BadRequest(new { message = "Failed to create menu item" });
        return Ok(new { message = "Menu item created successfully" });
    }

    [HttpPut("menu/{id}")]
    public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] CreateMenuItemRequest request)
    {
        var result = await _chefService.UpdateMenuItem(id, request);
        if (!result) return NotFound(new { message = "Menu item not found" });
        return Ok(new { message = "Menu item updated successfully" });
    }

    [HttpDelete("menu/{id}")]
    public async Task<IActionResult> DeleteMenuItem(int id)
    {
        var result = await _chefService.DeleteMenuItem(id);
        if (!result) return NotFound(new { message = "Menu item not found" });
        return Ok(new { message = "Menu item deleted successfully" });
    }

    [HttpPatch("inventory/{inventoryId}/stock")]
    public async Task<IActionResult> UpdateStock(int inventoryId, [FromBody] StockUpdateRequest request)
    {
        var result = await _chefService.UpdateStock(inventoryId, request.NewQuantity);
        if (!result) return NotFound(new { message = "Inventory record not found" });
        return Ok(new { message = "Stock updated successfully" });
    }

    [HttpPost("ingredients")]
    public async Task<IActionResult> CreateIngredient([FromBody] IngredientDto request)
    {
        var success = await _chefService.CreateIngredient(request);
        if (!success) return BadRequest(new { message = "Failed to create ingredient" });
        return Ok(new { message = "Ingredient created successfully" });
    }

    [HttpPut("ingredients/{id}")]
    public async Task<IActionResult> UpdateIngredient(int id, [FromBody] IngredientDto request)
    {
        var result = await _chefService.UpdateIngredient(id, request);
        if (!result) return NotFound(new { message = "Ingredient not found" });
        return Ok(new { message = "Ingredient updated successfully" });
    }

    [HttpDelete("ingredients/{id}")]
    public async Task<IActionResult> DeleteIngredient(int id)
    {
        var result = await _chefService.DeleteIngredient(id);
        if (!result) return NotFound(new { message = "Ingredient not found" });
        return Ok(new { message = "Ingredient deleted successfully" });
    }
}

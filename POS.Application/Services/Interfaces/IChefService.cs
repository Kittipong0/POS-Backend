using POS.Application.Models.Menu;
using POS.Application.Models.Orders;
using POS.Application.Models.Inventory;
using POS.Infrastructure.Services.Interfaces;

namespace POS.Application.Services.Interfaces
{
    public interface IChefService
    {
        Task<IEnumerable<KitchenQueueDto>> GetKitchenQueue();
        Task<IEnumerable<ChefOrderItemDto>> GetActiveOrdersDetailed();
        Task<IEnumerable<ChefInventoryDto>> GetInventoryDetails();
        Task<IEnumerable<IngredientDto>> GetAllIngredients();
        Task<bool> MarkItemCooking(int orderItemId);
        Task<bool> MarkItemsCooking(List<int> orderItemIds);
        Task<bool> MarkItemReady(int orderItemId);
        Task<bool> MarkItemsReady(List<int> orderItemIds);
        Task<bool> ToggleMenuAvailability(int menuItemId);
        
        // AI Menu Management
        Task<AiMenuSuggestionDto?> AnalyzeMenuItem(string? base64Image, string? menuName);
        Task<bool> CreateMenuItem(CreateMenuItemRequest request);
        Task<bool> UpdateMenuItem(int id, CreateMenuItemRequest request);
        Task<bool> DeleteMenuItem(int id);
        Task<bool> UpdateStock(int inventoryId, int newQuantity);
        
        // Ingredient Management
        Task<bool> CreateIngredient(IngredientDto request);
        Task<bool> UpdateIngredient(int id, IngredientDto request);
        Task<bool> DeleteIngredient(int id);
    }
}

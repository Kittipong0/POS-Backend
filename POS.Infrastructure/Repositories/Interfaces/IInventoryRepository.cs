using POS.Domain.Entities;

namespace POS.Infrastructure.Repositories.Interfaces
{
    public interface IInventoryRepository : IBaseRepository<Inventory>
    {
        IEnumerable<Inventory> GetByIds(IEnumerable<int> ids);

        // MenuItemIngredient operations
        IEnumerable<MenuItemIngredient> GetIngredientsByMenuItemId(int menuItemId);
        IEnumerable<MenuItemIngredient> GetIngredientsByMenuItemIds(IEnumerable<int> menuItemIds);
        void AddIngredient(MenuItemIngredient ingredient);
        void RemoveIngredientsByMenuItemId(int menuItemId);
    }
}

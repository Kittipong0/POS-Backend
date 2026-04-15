using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Data;
using POS.Infrastructure.Repositories.Interfaces;

namespace POS.Infrastructure.Repositories
{
    public class InventoryRepository : BaseRepository<Inventory>, IInventoryRepository
    {
        public InventoryRepository(AppDbContext writeContext, AppReadDbContext readContext)
            : base(writeContext, readContext)
        {
        }

        public override IEnumerable<Inventory> GetAll()
        {
            return _readContext.Inventories
                .Include(i => i.Category)
                .AsNoTracking()
                .ToList();
        }

        public override Inventory? GetById(int id)
        {
            return _readContext.Inventories
                .Include(i => i.Category)
                .AsNoTracking()
                .FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<Inventory> GetByIds(IEnumerable<int> ids)
        {
            return _readContext.Inventories
                .Include(i => i.Category)
                .AsNoTracking()
                .Where(i => ids.Contains(i.Id))
                .ToList();
        }

        // ── MenuItemIngredient operations ──

        public IEnumerable<MenuItemIngredient> GetIngredientsByMenuItemId(int menuItemId)
        {
            return _readContext.MenuItemIngredients
                .Include(mi => mi.Inventory)
                .Where(mi => mi.MenuItemId == menuItemId)
                .ToList();
        }

        public IEnumerable<MenuItemIngredient> GetIngredientsByMenuItemIds(IEnumerable<int> menuItemIds)
        {
            return _readContext.MenuItemIngredients
                .Include(mi => mi.Inventory)
                .Where(mi => menuItemIds.Contains(mi.MenuItemId))
                .ToList();
        }

        public void AddIngredient(MenuItemIngredient ingredient)
        {
            _writeContext.MenuItemIngredients.Add(ingredient);
        }

        public void RemoveIngredientsByMenuItemId(int menuItemId)
        {
            var existing = _writeContext.MenuItemIngredients.Where(mi => mi.MenuItemId == menuItemId).ToList();
            _writeContext.MenuItemIngredients.RemoveRange(existing);
        }
    }
}

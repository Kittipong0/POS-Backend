using POS.Domain.Entities;
using POS.Infrastructure.Data;
using POS.Infrastructure.Repositories.Interfaces;

namespace POS.Infrastructure.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly AppReadDbContext _context;

        public MenuItemRepository(AppReadDbContext context)
        {
            _context = context;
        }

        public MenuItem? GetById(int id)
        {
            return _context.MenuItems.Find(id);
        }

        public IEnumerable<MenuItem> GetListByIds(IEnumerable<int> ids)
        {
            return _context.MenuItems.Where(m => ids.Contains(m.Id)).ToList();
        }
    }
}

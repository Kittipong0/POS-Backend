using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infrastructure.Data;
using POS.Infrastructure.Repositories.Interfaces;

namespace POS.Infrastructure.Repositories
{
    public class MenuItemRepository : BaseRepository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(AppDbContext writeContext, AppReadDbContext readContext)
            : base(writeContext, readContext)
        {
        }

        public override IEnumerable<MenuItem> GetAll()
        {
            return _readContext.MenuItems
                .Include(m => m.Category)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<MenuItem> GetListByIds(IEnumerable<int> ids)
        {
            return _readContext.MenuItems
                .AsNoTracking()
                .Where(m => ids.Contains(m.Id))
                .ToList();
        }

        /// <summary>
        /// Returns a composable IQueryable for filtered menu queries.
        /// Caller applies pagination (Skip/Take) and materializes.
        /// </summary>
        public IQueryable<MenuItem> QueryMenu(
            string? keyword,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            bool onlyRecommended)
        {
            var query = _readContext.MenuItems
                .Include(m => m.Category)
                .Where(m => m.IsAvailable)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var lower = keyword.Trim().ToLower();
                query = query.Where(m =>
                    m.Name.ToLower().Contains(lower) ||
                    (m.NameEn != null && m.NameEn.ToLower().Contains(lower)) ||
                    m.Description.ToLower().Contains(lower));
            }

            if (categoryId.HasValue)
                query = query.Where(m => m.CategoryId == categoryId.Value);

            if (minPrice.HasValue)
                query = query.Where(m => m.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(m => m.Price <= maxPrice.Value);

            if (onlyRecommended)
                query = query.Where(m => m.IsRecommended || m.SalesCount > 50);

            return query;
        }
    }
}

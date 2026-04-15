using POS.Domain.Entities;

namespace POS.Infrastructure.Repositories.Interfaces
{
    public interface IMenuItemRepository : IBaseRepository<MenuItem>
    {
        IEnumerable<MenuItem> GetListByIds(IEnumerable<int> ids);

        /// <summary>
        /// Filtered query for paginated menu with search, category, and price filters.
        /// </summary>
        IQueryable<MenuItem> QueryMenu(string? keyword, int? categoryId, decimal? minPrice, decimal? maxPrice, bool onlyRecommended);
    }
}

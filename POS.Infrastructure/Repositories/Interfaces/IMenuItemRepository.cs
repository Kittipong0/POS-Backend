using POS.Domain.Entities;

namespace POS.Infrastructure.Repositories.Interfaces
{
    public interface IMenuItemRepository
    {
        MenuItem? GetById(int id);
        IEnumerable<MenuItem> GetListByIds(IEnumerable<int> ids);
    }
}

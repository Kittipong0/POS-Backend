using POS.Domain.Entities;
using POS.Infrastructure.Data;
using POS.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace POS.Infrastructure.Repositories
{
    public class TableRepository : BaseRepository<Table>, ITableRepository
    {
        public TableRepository(AppDbContext writeContext, AppReadDbContext readContext) 
            : base(writeContext, readContext)
        {
        }
    }
}

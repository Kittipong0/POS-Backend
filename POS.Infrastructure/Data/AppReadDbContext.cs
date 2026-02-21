using Microsoft.EntityFrameworkCore;

namespace POS.Infrastructure.Data;

public class AppReadDbContext : AppDbContext
{
    public AppReadDbContext(DbContextOptions<AppReadDbContext> options) : base(options)
    {
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.Application.Services;
using POS.Infrastructure.Data;
using POS.Infrastructure.Repositories;

namespace POS.API;

public static class DependencyConfig
{
    public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // DB Context
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Services
        services.AddScoped<IOrderService, OrderService>();
    }
}


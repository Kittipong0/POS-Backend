using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.Application.Services;
using POS.Application.Services.Interfaces;
using POS.Infrastructure.Data;
using POS.Infrastructure.Repositories;
using POS.Infrastructure.Repositories.Interfaces;
using POS.Infrastructure.Services.Interfaces;

namespace POS.API;

public static class DependencyConfig
{
    public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // DB Contexts
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddDbContext<AppReadDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Services
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IStaffService, StaffService>();
        services.AddScoped<IChefService, ChefService>();
        
        // AI Services - Use WinHttpHandler (native Windows HTTP stack, same as curl.exe)
        // This resolves SSL connection aborts caused by local security software
        // intercepting .NET's managed SocketsHttpHandler
        services.AddHttpClient<IAiService, POS.Infrastructure.Services.GeminiService>()
            .ConfigurePrimaryHttpMessageHandler(() => new System.Net.Http.WinHttpHandler
            {
                ServerCertificateValidationCallback = (message, cert, chain, errors) => true,
                WindowsProxyUsePolicy = System.Net.Http.WindowsProxyUsePolicy.DoNotUseProxy
            });
    }
}


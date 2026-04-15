using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;

namespace POS.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<MenuCategory> MenuCategories { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<InventoryCategory> InventoryCategories { get; set; }
    public DbSet<MenuItemIngredient> MenuItemIngredients { get; set; }
    public DbSet<Table> Tables { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Order / OrderItem ──
        modelBuilder.Entity<OrderItem>().HasKey(oi => oi.Id);
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);

        // ── MenuItem → MenuCategory ──
        modelBuilder.Entity<MenuItem>()
            .HasOne(m => m.Category)
            .WithMany(c => c.MenuItems)
            .HasForeignKey(m => m.CategoryId);

        // ── MenuItemIngredient join table (many-to-many) ──
        modelBuilder.Entity<MenuItemIngredient>()
            .HasOne(mi => mi.MenuItem)
            .WithMany(m => m.Ingredients)
            .HasForeignKey(mi => mi.MenuItemId);

        modelBuilder.Entity<MenuItemIngredient>()
            .HasOne(mi => mi.Inventory)
            .WithMany(i => i.MenuItemIngredients)
            .HasForeignKey(mi => mi.InventoryId);

        // ── Inventory → InventoryCategory ──
        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Category)
            .WithMany(c => c.Inventories)
            .HasForeignKey(i => i.CategoryId);

        // Seed data is managed via seed_data.sql
    }
}

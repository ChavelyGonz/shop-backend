using Domain.Models;

using Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Persistence.Contexts
{
    public class ShopDbContext : IdentityDbContext<User>
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options)
            : base(options) { }

        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<ProductStore> ProductsStores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplyModelsConfiguration(modelBuilder);
        }
        private static void ApplyModelsConfiguration(ModelBuilder modelBuilder)
        {
            new UserConfiguration().Configure(modelBuilder.Entity<User>());
            new StoreConfiguration().Configure(modelBuilder.Entity<Store>());
            new ProductConfiguration().Configure(modelBuilder.Entity<Product>());
            new PurchaseConfiguration().Configure(modelBuilder.Entity<Purchase>());
            new ProductStoreConfiguration().Configure(modelBuilder.Entity<ProductStore>());
        }
    }
}
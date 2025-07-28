using Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProductStoreConfiguration : IEntityTypeConfiguration<ProductStore>
    {
        public void Configure(EntityTypeBuilder<ProductStore> builder)
        {
            builder.HasKey(x => new { x.StoreId, x.ProductId });

            builder.HasOne(p => p.Store)
                .WithMany(ps => ps.Products)
                .HasForeignKey(ps => ps.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Product)
                .WithMany(ps => ps.Stores)
                .HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
using Domain.Enums;
using Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(p => p.Name)
                .IsUnique();
            builder.Property(p => p.Price)
                .HasDefaultValue(0.0f);
            
            builder.HasMany(p => p.Stores)
                .WithOne(ps => ps.Product)
                .HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.SetNull); 

            builder.Property(p => p.Unit)
                .HasConversion<string>()
                .HasDefaultValue(UnitOfMeasurement.kg);
        }
    }
}
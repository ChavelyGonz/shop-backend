using Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(p => p.Name)
                .IsUnique();

            builder.HasMany(p => p.Products)
                .WithOne(ps => ps.Store)
                .HasForeignKey(ps => ps.StoreId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
using Domain.Enums;
using Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.HasKey(v => new { v.ClientId, v.When });

            builder.Property(v => v.EmployeeId).IsRequired();

            builder.Property(v => v.When).IsRequired();
            builder.Property(v => v.When).HasConversion<string>();
            builder.Property(v => v.DescriptionText).HasColumnType("TEXT");

            builder.HasOne(v => v.Client)
                .WithMany(c => c.Purchases)
                .HasForeignKey(v => v.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.Employee)
                .WithMany()
                .HasForeignKey(v => v.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull); 
            
            builder.Ignore(p => p.Description);
            builder.Ignore(p => p.TotalAmount);

        }
    }
}
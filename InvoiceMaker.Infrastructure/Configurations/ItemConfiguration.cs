using InvoiceMaker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceMaker.Infrastructure.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.Price)
                .IsRequired()
                .HasPrecision(7, 2);

            builder.Property(x => x.Name)
                .IsRequired();
        }
    }
}

using InvoiceMaker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceMaker.Infrastructure.Configurations
{
    internal class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {

            builder.Property(x => x.City)
                .IsRequired();

            builder.Property(x => x.IssueDate)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.SaleDate)
                .IsRequired();

            builder.Property(x => x.Number)
                .IsRequired();

            builder.Property(x => x.Total)
                .HasPrecision(10, 2)
                .IsRequired();

            builder.OwnsOne(x => x.Vendor);

            builder.OwnsOne(x => x.Vendee);

            builder.HasMany(x => x.Items)
                .WithOne(i => i.Invoice)
                .HasForeignKey(i => i.InvoiceId);

            builder.Property(x => x.PaymentMethod)
                .IsRequired();
        }
    }
}

using InvoiceMaker.Domain.Entities;
using InvoiceMaker.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace InvoiceMaker.Infrastructure
{
    public class InvoiceDbContext : DbContext
    {
        private const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=InvoiceDb;Trusted_Connection=True;";

        public InvoiceDbContext() : base() { }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(InvoiceConfiguration).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}

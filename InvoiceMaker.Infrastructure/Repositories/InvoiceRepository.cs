using InvoiceMaker.Domain.Entities;
using InvoiceMaker.Domain.Helpers;
using InvoiceMaker.Domain.Interfaces;
using InvoiceMaker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceMaker.Infrastructure.Repositories
{
    public class InvoiceRepository : IRepository
    {
        public int GetInvoiceCount(int month, int year)
        {
            int result = 0;
            using (var dbContext = new InvoiceDbContext())
            {
                result = dbContext.Invoices
                            .AsNoTracking()
                            .Where(x => x.IssueDate.Month == month && x.IssueDate.Year == year)
                            .Count();
            }

            return result;
        }

        public void CreateInvoice(Invoice invoice)
        {
            using (var dbContext = new InvoiceDbContext())
            {
                dbContext.Invoices.Add(invoice);
                dbContext.SaveChanges();
            }
        }

        public Invoice GetInvoiceById(int id)
        {
            using (var dbContext = new InvoiceDbContext())
            {
                var invoice = dbContext.Invoices
                                .Include(x => x.Items)
                                .AsNoTracking()
                                .FirstOrDefault(x => x.Id == id);

                if (invoice is null)
                {
                    Console.WriteLine("Nie znaleziono faktury w bazie danych!");
                    return null;
                }

                return invoice;
            }
        }

        public List<Invoice> GetInvoices()
        {
            using (var dbContext = new InvoiceDbContext())
            {
                return dbContext.Invoices
                    .Include(x => x.Items)
                    .AsNoTracking()
                    .ToList();
            }

        }

        public void UpdateInvoice(UpdateInvoiceDto dto, Invoice invoice)
        {
            if (dto is null || invoice is null) return;

            using (var dbContext = new InvoiceDbContext())
            {
                ReflextionHelper.MapProperties(dto, invoice);
                dbContext.Entry(invoice).State = EntityState.Modified;
                dbContext.SaveChanges();
                Console.WriteLine("Faktura została zmodyfikowana!");
            }
        }

        public void DeleteInvoice(int id)
        {
            using (var dbContext = new InvoiceDbContext())
            {
                var invoice = dbContext.Invoices
                    .Include(x => x.Items)
                    .FirstOrDefault(x => x.Id == id);

                if (invoice is null)
                {
                    Console.WriteLine("Nie znaleziono faktury w bazie danych!");
                    return;
                }

                dbContext.Invoices.Remove(invoice);
                dbContext.SaveChanges();
            }
        }
    }
}

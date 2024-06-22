using InvoiceMaker.Domain.Entities;
using InvoiceMaker.Domain.Models;

namespace InvoiceMaker.Domain.Interfaces
{
    public interface IRepository
    {
        int GetInvoiceCount(int month, int year);
        Invoice GetInvoiceById(int id);
        void CreateInvoice(Invoice invoice);
        List<Invoice> GetInvoices();
        void UpdateInvoice(UpdateInvoiceDto dto, Invoice invoice);
        void DeleteInvoice(int id);
    }
}

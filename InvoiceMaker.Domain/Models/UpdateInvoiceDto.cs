using InvoiceMaker.Domain.Entities;
using InvoiceMaker.Domain.Enums;

namespace InvoiceMaker.Domain.Models
{
    public class UpdateInvoiceDto
    {
        public string? City { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? SaleDate { get; set; }
        public string? Number { get; set; }
        public decimal? Total { get; set; }

        public Vendor? Vendor { get; set; }
        public Vendee? Vendee { get; set; }

        public IEnumerable<Item>? Items { get; set; }

        public PaymentMethod? PaymentMethod { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? AccountNumber { get; set; }
    }
}

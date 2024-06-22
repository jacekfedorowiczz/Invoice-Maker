using InvoiceMaker.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceMaker.Domain.Entities
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime SaleDate { get; set; }

        public string Number { get; set; }

        public decimal Total { get; set; }

        public Vendor Vendor { get; set; }

        public Vendee Vendee { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();

        public PaymentMethod PaymentMethod { get; set; }

        public DateTime PaymentDate { get; set; }

        [StringLength(28)]
        public string? AccountNumber { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace InvoiceMaker.Domain.Entities
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }

        public int InvoiceId { get; set; }

        public virtual Invoice Invoice { get; set; }


        public Item(string name, int quantity, decimal price)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Quantity = quantity;
            Price = price;

            Amount = Price * Quantity;
        }
    }
}

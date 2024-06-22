using System.ComponentModel.DataAnnotations;
using InvoiceMaker.Domain.Interfaces;

namespace InvoiceMaker.Domain.Entities
{
    public class Vendee : IPerson
    {
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(12)]
        public string PhoneNumber { get; set; }
        public string? TaxNumber { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }

        public Vendee(string firstName, string lastName, string phoneNumber, string? taxNumber = null)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            TaxNumber = taxNumber;
        }
    }
}

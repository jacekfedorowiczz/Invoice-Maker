using InvoiceMaker.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace InvoiceMaker.Domain.Entities
{
    public class Vendor : IPerson
    {
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(12)]
        public string PhoneNumber { get; set; }
        public string TaxNumber { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }

        public Vendor(string firstName, string lastName, string phoneNumber, string taxNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            TaxNumber = taxNumber;
        }
    }
}

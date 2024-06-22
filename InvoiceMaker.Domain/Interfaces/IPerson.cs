namespace InvoiceMaker.Domain.Interfaces
{
    public interface IPerson
    {
        string City { get; set; }
        string Country { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string PhoneNumber { get; set; }
        string PostalCode { get; set; }
        string Street { get; set; }
        string? TaxNumber { get; set; }
    }
}
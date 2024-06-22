namespace InvoiceMaker.Domain.Entities
{
    public class Company(string name, string form, string representation, string city, string street, string streetNumber, string postalCode)
    {
        public string Name { get; set; } = name;
        public string Form { get; set; } = form;
        public string Representation { get; set; } = representation;
        public string City { get; set; } = city;
        public string Street { get; set; } = street;
        public string StreetNumber { get; set; } = streetNumber;
        public string PostalCode { get; set; } = postalCode;
    }
}

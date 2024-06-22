using InvoiceMaker.Domain.Entities;
using InvoiceMaker.Domain.Enums;

namespace InvoiceMaker.Domain.Interfaces
{
    public interface IBuilder
    {
        Invoice Build();
        IBuilder SetCity(string city);
        IBuilder SetSaleDate(DateTime date);
        IBuilder SetVendor(Vendor vendor);
        IBuilder SetVendee(Vendee vendee);
        IBuilder SetItems(List<Item> Items);
        IBuilder SetPaymentMethod(PaymentMethod paymentMethod);
        IBuilder SetPaymentDetails(DateTime paymentDate, string? accountNumber);
    }
}

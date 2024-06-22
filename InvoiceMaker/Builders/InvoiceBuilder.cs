using InvoiceMaker.Domain.Entities;
using InvoiceMaker.Domain.Enums;
using InvoiceMaker.Domain.Interfaces;

namespace InvoiceMaker.Builders
{
    public class InvoiceBuilder : IBuilder
    {
        private Invoice _invoice = new Invoice();
        private readonly IRepository _repository;

        public InvoiceBuilder(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Invoice Build()
        {
            SetNumber();
            CalculateTotal();
            return _invoice;
        }

        public IBuilder SetItems(List<Item> items)
        {
            _invoice.Items = items;
            return this;
        }

        public IBuilder SetCity(string city)
        {
            _invoice.City = city;
            return this;
        }

        public IBuilder SetPaymentMethod(PaymentMethod paymentMethod)
        {
            _invoice.PaymentMethod = paymentMethod;
            return this;
        }

        public IBuilder SetSaleDate(DateTime saleDate)
        {
            _invoice.SaleDate = saleDate;
            return this;
        }

        public IBuilder SetVendee(Vendee vendee)
        {
            _invoice.Vendee = vendee;
            return this;
        }

        public IBuilder SetVendor(Vendor vendor)
        {
            _invoice.Vendor = vendor;
            return this;
        }

        public IBuilder SetPaymentDetails(DateTime paymentDate, string? accountNumber)
        {
            _invoice.PaymentDate = paymentDate;
            _invoice.AccountNumber = string.IsNullOrEmpty(accountNumber)
                ? string.Empty
                : accountNumber;
            return this;
        }

        private void SetNumber()
        {
            var actualDate = DateTime.UtcNow;
            var invoiceCount = _repository.GetInvoiceCount(actualDate.Month, actualDate.Year);

            _invoice.Number = $"FV/{++invoiceCount}/{actualDate.Month}/{actualDate.Year}";
        }

        public void CalculateTotal()
        {
            if (_invoice.Items is null || _invoice.Items.Count == 0)
            {
                _invoice.Total = 0;
                return;
            }

            foreach (var item in _invoice.Items)
            {
                _invoice.Total += item.Amount;
            }
        }
    }
}

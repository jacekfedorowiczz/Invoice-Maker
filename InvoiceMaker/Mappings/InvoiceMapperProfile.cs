using AutoMapper;
using InvoiceMaker.Domain.Entities;
using InvoiceMaker.Domain.Models;

namespace InvoiceMaker.Mappings
{
    public class InvoiceMapperProfile : Profile
    {
        public InvoiceMapperProfile()
        {
            CreateMap<Invoice, InvoiceDto>()
                .ReverseMap();

            CreateMap<Invoice, UpdateInvoiceDto>()
                .ReverseMap();
        }
    }
}

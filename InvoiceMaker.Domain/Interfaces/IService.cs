using InvoiceMaker.Domain.Entities;
using InvoiceMaker.Domain.Models;

namespace InvoiceMaker.Domain.Interfaces
{
    public interface IService
    {
        void Create();
        void Update(int id);
        void Delete(int id);
        InvoiceDto GetById(int id);
        List<InvoiceDto> GetAll();
        Company GetDataFromKRS(string number, string registry);
    }
}

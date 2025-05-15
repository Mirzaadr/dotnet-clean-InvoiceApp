namespace InvoiceApp.Domain.Invoices;

public interface IInvoiceRepository
{
    Task<Invoice?> GetById(InvoiceId id);
    Task<List<Invoice>> GetAll();
    void Create(Invoice invoice);
    void Update(Invoice invoice);
    void Delete(Invoice invoice);
}
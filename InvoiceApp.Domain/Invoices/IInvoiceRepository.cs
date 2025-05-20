namespace InvoiceApp.Domain.Invoices;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(InvoiceId id);
    Task<List<Invoice>> GetAllAsync();
    Task AddAsync(Invoice invoice);
    Task UpdateAsync(Invoice invoice);
    Task DeleteAsync(Invoice invoice);
}

// public interface IInvoiceRepository
// {
//     Task<Invoice?> GetByIdAsync(InvoiceId id);
//     Task<List<Invoice>> GetByClientIdAsync(ClientId clientId);
//     Task<List<Invoice>> GetAllAsync();

//     Task AddAsync(Invoice invoice);
//     Task UpdateAsync(Invoice invoice);
//     Task DeleteAsync(Invoice invoice);
// }

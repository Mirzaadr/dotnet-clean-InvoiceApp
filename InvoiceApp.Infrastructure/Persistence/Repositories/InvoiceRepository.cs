using InvoiceApp.Domain.Invoices;

namespace InvoiceApp.Infrastructure.Persistence.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    // public static List<Invoice> invoices = new();
    private readonly InMemoryDbContext _context;

    public InvoiceRepository(InMemoryDbContext context)
    {
        _context = context;
    }

    public void Create(Invoice invoice)
    {
        var existingInvoice = _context.Invoices.FirstOrDefault(i => i.Id == invoice.Id);
        if (existingInvoice is null)
        {
            _context.Add(invoice);
        }
        else
        {
            existingInvoice = invoice;
        }
        _context.SaveChanges();
    }

    public void Delete(Invoice invoice)
    {
        var existingInvoice = _context.Invoices.FirstOrDefault(i => i.Id == invoice.Id);
        if (existingInvoice is null)
        {
            throw new Exception("Invoice not found");
        }
        else
        {
            _context.Invoices.Remove(invoice);
        }
        _context.SaveChanges();
    }

  public async Task<List<Invoice>> GetAll()
  {
        return await Task.FromResult(_context.Invoices);
  }

  public async Task<Invoice?> GetById(InvoiceId id)
    {
        var invoice =  _context.Invoices.FirstOrDefault(i => i.Id == id);
        return await Task.FromResult(invoice);
    }

    public void Update(Invoice invoice)
    {
        throw new NotImplementedException();
    }
}
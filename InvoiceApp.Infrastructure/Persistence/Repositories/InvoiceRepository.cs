using InvoiceApp.Domain.Commons.Models;
using InvoiceApp.Domain.Invoices;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InvoiceApp.Infrastructure.Persistence.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    // public static List<Invoice> invoices = new();
    private readonly InMemoryDbContext _context;

    public InvoiceRepository(InMemoryDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Invoice invoice)
    {
        await Task.CompletedTask.ContinueWith(t =>
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
        });
    }

    public async Task DeleteAsync(Invoice invoice)
    {
        await Task.CompletedTask.ContinueWith(t =>
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
        });
    }

    public async Task<PagedList<Invoice>> GetAllAsync(int page, int pageSize, string? searchTerm)
    {
        var invoiceQuery = _context.Invoices.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            invoiceQuery = invoiceQuery.Where(i =>
                i.InvoiceNumber.ToLower().Contains(searchTerm) ||
                i.Status.Value.ToLower().Contains(searchTerm) ||
                i.ClientName.ToLower().Contains(searchTerm)
            );
        }

        return await PagedList<Invoice>.CreateAsync(invoiceQuery, page, pageSize);
    }

  public Task<List<Invoice>> GetAllAsync()
  {
    throw new NotImplementedException();
  }

  public async Task<Invoice?> GetByIdAsync(InvoiceId id)
    {
        var invoice =  _context.Invoices.FirstOrDefault(i => i.Id == id);
        return await Task.FromResult(invoice);
    }

    public async Task<Invoice> GetLatestAsync()
    {
        var invoice = _context.Invoices.OrderByDescending(i => i.CreatedDate).First();
        return await Task.FromResult(invoice);
    }

    public Task UpdateAsync(Invoice invoice)
    {
        var currentInvoice =  _context.Invoices.FirstOrDefault(i => i.Id == invoice.Id);
        if (currentInvoice is null)
            throw new Exception("Invoice not found");
        currentInvoice.UpdateClient(invoice.ClientId.Value, invoice.ClientName ?? currentInvoice.ClientName);
        currentInvoice.UpdateItems(invoice.Items.ToList());

        _context.SaveChanges();
        return Task.CompletedTask;
    }

}
using InvoiceApp.Domain.Commons.Models;
using InvoiceApp.Domain.Invoices;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace InvoiceApp.Infrastructure.Persistence.Repositories.InMemory;

public class InvoiceInMemoryRepository : IInvoiceRepository
{
    // public static List<Invoice> invoices = new();
    private readonly InMemoryDbContext _context;

    public InvoiceInMemoryRepository(InMemoryDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Invoice invoice)
    {
        await Task.CompletedTask.ContinueWith(t =>
        {
            _context.Invoices.Add(invoice);
        });
    }

    public async Task DeleteAsync(Invoice invoice)
    {
        await Task.CompletedTask.ContinueWith(t =>
        {
            _context.Invoices.Remove(invoice);
        });
    }

    public async Task<PagedList<Invoice>> GetAllAsync(
        int page,
        int pageSize,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder
    )
    {
        var invoiceQuery = _context.Invoices.AsQueryable();

        // TODO: implement better filtering system
        if (!string.IsNullOrEmpty(searchTerm))
        {
            // Split by comma, trim whitespace, and lowercase
            var searchTerms = searchTerm
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(term => term.Trim().ToLower())
                .ToList();

            // AND-based filtering: all terms must match one of the fields
            foreach (var term in searchTerms)
            {
                invoiceQuery = term switch
                {
                    "draft" => invoiceQuery.Where(i => i.Status == InvoiceStatus.Draft),
                    "sent" => invoiceQuery.Where(i => i.Status == InvoiceStatus.Sent),
                    "paid" => invoiceQuery.Where(i => i.Status == InvoiceStatus.Paid),
                    _ => invoiceQuery.Where(i =>
                        i.InvoiceNumber.ToLower().Contains(term) ||
                        i.ClientName.ToLower().Contains(term)
                    )
                };
            }
        }

        Expression<Func<Invoice, object>> keySelector = sortColumn?.ToLower() switch
        {
            "number" => i => i.InvoiceNumber,
            "issueDate" => i => i.IssueDate,
            "dueDate" => i => i.DueDate,
            "createdDate" => i => i.CreatedDate!,
            "updatedDate" => i => i.UpdatedDate!,
            "total" => i => i.TotalAmount,
            "client_name" => i => i.ClientName,
            _ => i => i.InvoiceNumber,
        };

        if (sortOrder?.ToLower() == "desc")
        {
            invoiceQuery = invoiceQuery.OrderByDescending(keySelector);
        }
        else
        {
            invoiceQuery = invoiceQuery.OrderBy(keySelector);
        }

        return await PagedList<Invoice>.CreateAsync(invoiceQuery, page, pageSize);
    }

    public Task<List<Invoice>> GetAllAsync()
    {
        return Task.FromResult(_context.Invoices);
    }

    public async Task<Invoice?> GetByIdAsync(InvoiceId id)
    {
        var invoice = _context.Invoices.FirstOrDefault(i => i.Id == id);
        return await Task.FromResult(invoice);
    }

    public async Task<Invoice> GetLatestAsync()
    {
        var invoice = _context.Invoices.OrderByDescending(i => i.CreatedDate).First();
        return await Task.FromResult(invoice);
    }

    public Task<double> GetTotalAmountAsync()
    {
        var totalRevenue = _context.Invoices
            .SelectMany(i => i.Items)
            .Sum(ii => ii.Quantity * ii.UnitPrice);
        return Task.FromResult(totalRevenue);
    }

    public Task UpdateAsync(Invoice invoice)
    {
        // _context.Invoices.Update(invoice);
        return Task.CompletedTask;
    }

}
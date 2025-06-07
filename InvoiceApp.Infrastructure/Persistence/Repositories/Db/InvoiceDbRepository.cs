using InvoiceApp.Domain.Commons.Models;
using InvoiceApp.Domain.Invoices;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace InvoiceApp.Infrastructure.Persistence.Repositories.Db;

public class InvoiceDbRepository : IInvoiceRepository
{
    // public static List<Invoice> invoices = new();
    // private readonly AppDbContext _context;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public InvoiceDbRepository(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        // _context = context;
        _dbContextFactory = dbContextFactory;
    }

    public async Task AddAsync(Invoice invoice)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            dbContext.Invoices.Add(invoice);
            dbContext.SaveChanges();
        }
    }

    public async Task DeleteAsync(Invoice invoice)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            dbContext.Invoices.Remove(invoice);
            dbContext.SaveChanges();
        }
    }

    public async Task<PagedList<Invoice>> GetAllAsync(
        int page,
        int pageSize,
        string? searchTerm,
        string? sortColumn,
        string? sortOrder
    )
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            
            var invoiceQuery = dbContext.Invoices.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                invoiceQuery = invoiceQuery.Where(i =>
                    i.InvoiceNumber.ToLower().Contains(searchTerm) ||
                    i.Status.Value.ToLower().Contains(searchTerm) ||
                    i.ClientName.ToLower().Contains(searchTerm)
                );
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
    }

    public async Task<List<Invoice>> GetAllAsync()
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            return await dbContext.Invoices.ToListAsync();
        }
    }

    public async Task<Invoice?> GetByIdAsync(InvoiceId id)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            var invoice =  dbContext.Invoices.FirstOrDefault(i => i.Id == id);
            return await Task.FromResult(invoice);
        }
    }

    public async Task<Invoice> GetLatestAsync()
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            var invoice = dbContext.Invoices.OrderByDescending(i => i.CreatedDate).First();
            return await Task.FromResult(invoice);
        }
    }

    public async Task<double> GetTotalAmountAsync()
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            var totalRevenue = await dbContext.Invoices
                .SelectMany(i => i.Items)
                .SumAsync(ii => ii.Quantity * ii.UnitPrice);
            return totalRevenue;
        }
    }

    public async Task UpdateAsync(Invoice invoice)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            var currentInvoice = dbContext.Invoices.FirstOrDefault(i => i.Id == invoice.Id);
            if (currentInvoice is null)
                throw new Exception("Invoice not found");
            currentInvoice.UpdateInvoiceDates(invoice.IssueDate, invoice.DueDate);
            currentInvoice.UpdateClient(invoice.ClientId.Value, invoice.ClientName ?? currentInvoice.ClientName);
            currentInvoice.UpdateItems(invoice.Items.ToList());

            dbContext.SaveChanges();
            // return Task.CompletedTask;
        }
    }

}
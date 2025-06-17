using InvoiceApp.Domain.Commons.Models;
using InvoiceApp.Domain.Invoices;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace InvoiceApp.Infrastructure.Persistence.Repositories.Db;

public class InvoiceDbRepository : IInvoiceRepository
{
    private readonly AppDbContext _context;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public InvoiceDbRepository(IDbContextFactory<AppDbContext> dbContextFactory,  AppDbContext context)
    {
        _context = context;
        _dbContextFactory = dbContextFactory;
    }

    public async Task AddAsync(Invoice invoice)
    {
        await _context.Invoices.AddAsync(invoice);
    }

    public Task DeleteAsync(Invoice invoice)
    {
        _context.Invoices.Remove(invoice);
        return Task.CompletedTask;
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

            // TODO: implement better filtering system
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Split by comma, trim whitespace, and lowercase
                var searchTerms = searchTerm
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(term => term.Trim().ToLower())
                    .ToList();

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
        var invoice =  await _context.Invoices.FirstOrDefaultAsync(i => i.Id == id);
        return invoice;

    }

    public async Task<Invoice> GetLatestAsync()
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            return await dbContext.Invoices.OrderByDescending(i => i.CreatedDate).FirstAsync();
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

    public Task UpdateAsync(Invoice invoice)
    {
        _context.Set<Invoice>().Update(invoice);
        // await _context.SaveChangesAsync();
        return Task.CompletedTask;
    }

}
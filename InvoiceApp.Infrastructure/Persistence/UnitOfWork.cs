namespace InvoiceApp.Infrastructure.Persistence;


public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly InMemoryDbContext _context;

    public UnitOfWork(InMemoryDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // In-memory context does not require saving changes
        return Task.FromResult(0);
    }

    public void Dispose()
    {
        // _context?.Dispose();
    }
}
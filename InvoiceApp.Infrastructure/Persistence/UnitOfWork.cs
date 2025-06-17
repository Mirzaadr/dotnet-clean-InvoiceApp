namespace InvoiceApp.Infrastructure.Persistence;


public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // In-memory context does not require saving changes
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
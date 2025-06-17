using InvoiceApp.Application.Commons.Interface;
using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Infrastructure.Persistence.Repositories.InMemory;

public class ClientInMemoryRepository : IClientRepository
{
    private readonly InMemoryDbContext _context;
    private readonly ICacheService _cache;
    private static readonly string CacheKey = "clients_cache";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    public ClientInMemoryRepository(InMemoryDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public Task AddAsync(Client client)
    {
        _context.Clients.Add(client);
        // Clear cache after adding new client
        _cache.Remove(CacheKey);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Client client)
    {
        _context.Clients.Remove(client);
        // Clear cache after deleting client
        _cache.Remove(CacheKey);

        return Task.CompletedTask;
    }

    public async Task<List<Client>> GetAllAsync() {
        var clients = await _cache.GetOrCreateAsync(
            CacheKey, 
            () => Task.FromResult(_context.Clients), CacheDuration);
        return clients;
    }
    
    public async Task<PagedList<Client>> GetAllAsync(int page, int pageSize, string? searchTerm)
    {
        var clientQuery = _context.Clients.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            clientQuery = clientQuery.Where(i =>
                i.Name.ToLower().Contains(searchTerm) ||
                i.Address != null && i.Address.ToLower().Contains(searchTerm) ||
                i.Email != null && i.Email.ToLower().Contains(searchTerm)
            );
        }

        return await PagedList<Client>.CreateAsync(clientQuery, page, pageSize);
    }
    
    public async Task<Client?> GetByIdAsync(ClientId id)
    {
        var client = _context.Clients.FirstOrDefault(i => i.Id == id);
        return await Task.FromResult(client);
    }

    public async Task UpdateAsync(Client client)
    {
        // _context.Clients.Update(client);
        _cache.Remove(CacheKey);
        await Task.CompletedTask;
    }
}
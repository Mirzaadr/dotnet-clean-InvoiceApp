using InvoiceApp.Application.Commons.Interface;
using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Commons.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Persistence.Repositories.Db;

public class ClientDbRepository : IClientRepository
{
    private readonly AppDbContext _context;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly ICacheService _cache;
    private static readonly string CacheKey = "clients_cache";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    public ClientDbRepository(ICacheService cache, AppDbContext context, IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _context = context;
        _cache = cache;
        _dbContextFactory = dbContextFactory;
    }

    public async Task AddAsync(Client client)
    {
        await _context.Clients.AddAsync(client);
        // await _context.SaveChangesAsync();

        // Clear cache after adding new client
        _cache.Remove(CacheKey);
        // return Task.CompletedTask;
    }

    public Task DeleteAsync(Client client)
    {
        _context.Clients.Remove(client);
        // await _context.SaveChangesAsync();

        // Clear cache after deleting client
        _cache.Remove(CacheKey);
        return Task.CompletedTask;
    }

    public async Task<List<Client>> GetAllAsync() {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            // Use the dbContext to access the database
            // This is useful if you want to ensure that the context is disposed of properly
            // and avoid issues with long-lived contexts in a web application.
            var clients = await _cache.GetOrCreateAsync(
                CacheKey, 
                async () => await dbContext.Clients.ToListAsync(), CacheDuration);
            return clients;
        }
    }
    
    public async Task<PagedList<Client>> GetAllAsync(int page, int pageSize, string? searchTerm)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {

            var clientQuery = dbContext.Clients.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                clientQuery = clientQuery.Where(i =>
                    i.Name.ToLower().Contains(searchTerm) ||
                    (i.Address != null && i.Address.ToLower().Contains(searchTerm)) ||
                    (i.Email != null && i.Email.ToLower().Contains(searchTerm))
                );
            }

            return await PagedList<Client>.CreateAsync(clientQuery, page, pageSize);
        }
    }
    
    public async Task<Client?> GetByIdAsync(ClientId id)
    {
        var client = _context.Clients.FirstOrDefault(i => i.Id == id);
        return await Task.FromResult(client);
    }

    public async Task UpdateAsync(Client client)
    {
        _context.Set<Client>().Update(client);
        _cache.Remove(CacheKey);
        await Task.CompletedTask;
    }
}
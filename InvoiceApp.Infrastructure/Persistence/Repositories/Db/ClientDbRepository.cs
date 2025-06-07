using InvoiceApp.Application.Commons.Interface;
using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Commons.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Persistence.Repositories.Db;

public class ClientDbRepository : IClientRepository
{
    // private readonly AppDbContext _context;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly ICacheService _cache;
    private static readonly string CacheKey = "clients_cache";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    public ClientDbRepository(ICacheService cache, IDbContextFactory<AppDbContext> dbContextFactory)
    {
        // _context = context;
        _cache = cache;
        _dbContextFactory = dbContextFactory;
    }

    public async Task AddAsync(Client client)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            dbContext.Clients.Add(client);
            await dbContext.SaveChangesAsync();

            // Clear cache after adding new client
            _cache.Remove(CacheKey);
            // return Task.CompletedTask;
        }
    }

    public async Task DeleteAsync(Client client)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            dbContext.Clients.Remove(client);
            await dbContext.SaveChangesAsync();

            // Clear cache after deleting client
            _cache.Remove(CacheKey);
            // return Task.CompletedTask;
        }
    }

    public async Task<List<Client>> GetAllAsync() {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            var clients = await _cache.GetOrCreateAsync(
                CacheKey, 
                async () => await dbContext.Clients.ToListAsync(), CacheDuration);
            // var clients = await _context.Clients.ToListAsync();
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
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            var client = dbContext.Clients.FirstOrDefault(i => i.Id == id);
            return await Task.FromResult(client);
        }
    }

    public async Task UpdateAsync(Client client)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            
            var existingClients = dbContext.Clients.FirstOrDefault(i => i.Id == client.Id);
            if (existingClients is null)
            {
                throw new KeyNotFoundException($"Client with ID {client.Id.ToString()} not found.");
            }

            existingClients.Update(
                client.Name,
                client.Address,
                client.Email,
                client.PhoneNumber,
                null
            );
            await dbContext.SaveChangesAsync();
            _cache.Remove(CacheKey);
            await Task.CompletedTask;
        }
    }
}
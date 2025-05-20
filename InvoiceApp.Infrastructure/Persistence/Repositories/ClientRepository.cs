using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Infrastructure.Persistence.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly InMemoryDbContext _context;

    public ClientRepository(InMemoryDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(Client client)
    {
        var existingClient = _context.Clients.FirstOrDefault(i => i.Id == client.Id);
        if (existingClient is null)
        {
            _context.Add(client);
        }
        else
        {
            existingClient = client;
        }
        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Client client)
    {
        var existingInvoice = _context.Clients.FirstOrDefault(i => i.Id == client.Id);
        if (existingInvoice is null)
        {
            throw new Exception("Invoice not found");
        }
        else
        {
            _context.Clients.Remove(client);
        }
        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task<List<Client>> GetAllAsync() => Task.FromResult(_context.Clients);
    
    public async Task<PagedList<Client>> GetAllAsync(int page, int pageSize, string? searchTerm)
    {
        var clientQuery = _context.Clients.AsQueryable();

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
    
    public async Task<Client?> GetByIdAsync(ClientId id)
    {
        var client = _context.Clients.FirstOrDefault(i => i.Id == id);
        return await Task.FromResult(client);
    }

    public Task UpdateAsync(Client client)
    {
        throw new NotImplementedException();
    }
}
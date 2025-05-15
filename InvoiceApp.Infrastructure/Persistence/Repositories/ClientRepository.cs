using InvoiceApp.Domain.Clients;

namespace InvoiceApp.Infrastructure.Persistence.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly InMemoryDbContext _context;

    public ClientRepository(InMemoryDbContext context)
    {
        _context = context;
    }

    public void Create(Client client)
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
    }

    public void Delete(Client client)
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
    }

    public async Task<Client?> GetById(ClientId id)
    {
        var client =  _context.Clients.FirstOrDefault(i => i.Id == id);
        return await Task.FromResult(client);
    }

    public void Update(Client client)
    {
        throw new NotImplementedException();
    }
}
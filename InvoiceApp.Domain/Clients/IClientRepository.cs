namespace InvoiceApp.Domain.Clients;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(ClientId id);
    Task<List<Client>> GetAllAsync();
    Task AddAsync(Client client);
    Task UpdateAsync(Client client);
    Task DeleteAsync(Client client);
}

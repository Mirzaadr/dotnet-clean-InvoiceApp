using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Domain.Clients;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(ClientId id);
    Task<List<Client>> GetAllAsync();
    Task<PagedList<Client>> GetAllAsync(int page, int pageSize, string? searchTerm);
    Task AddAsync(Client client);
    Task UpdateAsync(Client client);
    Task DeleteAsync(Client client);
}

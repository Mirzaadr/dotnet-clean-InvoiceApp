namespace InvoiceApp.Domain.Clients;

public interface IClientRepository
{
    Task<Client?> GetById(ClientId id);
    void Create(Client client);
    void Update(Client client);
    void Delete(Client client);
}
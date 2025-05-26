using InvoiceApp.Domain.Invoices;
using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Products;

namespace InvoiceApp.Infrastructure.Persistence;

public class InMemoryDbContext
{
    public List<Invoice> Invoices { get; set; } 
    public List<Product> Products { get; set; } 
    public List<Client> Clients { get; set; }

    public InMemoryDbContext()
    {
        Invoices = new List<Invoice>();
        Products = new List<Product>();
        Clients = new List<Client>();
    }

    public void Add<TEntity>(TEntity entity) where TEntity : class
    {
        if (typeof(TEntity) == typeof(Invoice) && entity is not null)
        {
            Invoices.Add((entity as Invoice)!);
        }
        else if (typeof(TEntity) == typeof(Product) && entity is not null)
        {
            Products.Add((entity as Product)!);
        }
        else if (typeof(TEntity) == typeof(Client) && entity is not null)
        {
            Clients.Add((entity as Client)!);
        }
    }

    public void SaveChanges()
    {}
}
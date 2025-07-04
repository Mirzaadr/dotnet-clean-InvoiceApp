using InvoiceApp.Domain.Invoices;
using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Products;
using InvoiceApp.Infrastructure.DomainEvents;
using InvoiceApp.Domain.Commons.Interfaces;
using InvoiceApp.Domain.Users;

namespace InvoiceApp.Infrastructure.Persistence;

public class InMemoryDbContext : IUnitOfWork
{
    private readonly IDomainEventsDispatcher _dispatcher;
    public List<Invoice> Invoices { get; set; } 
    public List<Product> Products { get; set; } 
    public List<Client> Clients { get; set; }
    public List<User> Users { get; set; }

    public InMemoryDbContext(IDomainEventsDispatcher dispatcher)
    {
        Invoices = new List<Invoice>();
        Products = new List<Product>();
        Clients = new List<Client>();
        Users = new List<User>();
        _dispatcher = dispatcher;
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
        else if (typeof(TEntity) == typeof(User) && entity is not null)
        {
            Users.Add((entity as User)!);
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await PublishDomainEventsAsync();

        return await Task.FromResult(1); // pretend a change occurred
    }

    public int SaveChanges()
    {
        return 0;
    }

    private async Task PublishDomainEventsAsync()
    {
        var allEntities = new List<Invoice>();
        allEntities.AddRange(Invoices);

        var domainEvents = allEntities
            .SelectMany(e =>
            {
                List<IDomainEvent> domainEvents = e.DomainEvents.ToList();
                e.ClearDomainEvents();

                return domainEvents;
            });

        await _dispatcher.DispatchAsync(domainEvents);
    }
}
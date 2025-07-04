using InvoiceApp.Domain.Invoices;
using InvoiceApp.Domain.Clients;
using Microsoft.EntityFrameworkCore;
using InvoiceApp.Domain.Products;
using InvoiceApp.Infrastructure.DomainEvents;
using InvoiceApp.Domain.Commons.Models;
using InvoiceApp.Domain.Commons.Interfaces;
using InvoiceApp.Domain.Users;

namespace InvoiceApp.Infrastructure.Persistence;

public class AppDbContext : DbContext, IUnitOfWork
{
    private readonly IDomainEventsDispatcher _dispatcher;
    public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventsDispatcher dispatcher) : base(options)
    {
        _dispatcher = dispatcher;
    }

    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
  
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await PublishDomainEventsAsync();

        int result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
  
    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<BaseEntity<ValueObject>>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                List<IDomainEvent> domainEvents = entity.DomainEvents.ToList();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        await _dispatcher.DispatchAsync(domainEvents);
    }
}
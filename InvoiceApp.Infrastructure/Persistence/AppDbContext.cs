using InvoiceApp.Domain.Invoices;
using InvoiceApp.Domain.Clients;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
  protected AppDbContext(DbContextOptions options) : base(options)
  {
  }

  public DbSet<Invoice> Invoices { get; set; }
  public DbSet<Client> Clients { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
      modelBuilder
          .ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
      base.OnModelCreating(modelBuilder);
  }
}
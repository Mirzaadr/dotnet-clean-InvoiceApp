using InvoiceApp.Domain.Invoices;
using InvoiceApp.Domain.Clients;
using Microsoft.EntityFrameworkCore;
using InvoiceApp.Domain.Products;

namespace InvoiceApp.Infrastructure.Persistence;

public class AppDbContext : DbContext, IUnitOfWork
{
  protected AppDbContext(DbContextOptions options) : base(options)
  {
  }

  public DbSet<Invoice> Invoices { get; set; }
  public DbSet<Client> Clients { get; set; }
  public DbSet<Product> Products { get; set; }
  
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
}
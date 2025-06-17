using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using InvoiceApp.Infrastructure.Services;
using InvoiceApp.Infrastructure.Persistence;
using InvoiceApp.Domain.Invoices;
using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Products;
using InvoiceApp.Application.Commons.Interface;
using Microsoft.EntityFrameworkCore;
using InvoiceApp.Infrastructure.Persistence.Repositories.InMemory;
using InvoiceApp.Infrastructure.Persistence.Repositories.Db;

namespace InvoiceApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        // services.AddInMemoryDatabase();
        services.AddDatabase(configuration);

        services.AddServices();


        return services;
    }

    private static IServiceCollection AddServices(
        this IServiceCollection services
    )
    {
        // Services
        services.AddSingleton<IInvoiceNumberGenerator, InvoiceNumberGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();
        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        services.AddDbContextFactory<AppDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("Default")
        ), ServiceLifetime.Scoped);
        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<AppDbContext>());


        // repository
        services.AddScoped<IInvoiceRepository, InvoiceDbRepository>();
        services.AddScoped<IClientRepository, ClientDbRepository>();
        services.AddScoped<IProductRepository, ProductDbRepository>();
        return services;
    }

    private static IServiceCollection AddInMemoryDatabase(
        this IServiceCollection services
    )
    {
        services.AddSingleton<InMemoryDbContext>();
        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<InMemoryDbContext>());

        // repository
        services.AddScoped<IInvoiceRepository, InvoiceInMemoryRepository>();
        services.AddScoped<IClientRepository, ClientInMemoryRepository>();
        services.AddScoped<IProductRepository, ProductInMemoryRepository>();
        return services;
    }
}

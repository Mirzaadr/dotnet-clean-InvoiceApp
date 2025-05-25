using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
using InvoiceApp.Infrastructure.Services;
using InvoiceApp.Infrastructure.Persistence;
using InvoiceApp.Domain.Invoices;
using InvoiceApp.Infrastructure.Persistence.Repositories;
using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Products;
using InvoiceApp.Application.Commons.Interface;
// using Microsoft.Extensions.Options;
// using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        // services.AddDbContext<AppDbContext>(options => 
        //     options
        //         .UseNpgsql(configuration.GetConnectionString("Database"))
        //         .UseSnakeCaseNamingConvention()
        // );
        services.AddSingleton<InMemoryDbContext>();

        // repository
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        // Services
        services.AddSingleton<IInvoiceNumberGenerator, InvoiceNumberGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();

        
        return services;
    }
}

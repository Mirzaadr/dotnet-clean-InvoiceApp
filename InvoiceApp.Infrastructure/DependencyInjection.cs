using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.IdentityModel.Tokens;
using System.Text;
using InvoiceApp.Infrastructure.Services;
using InvoiceApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using InvoiceApp.Domain.Invoices;
using InvoiceApp.Infrastructure.Persistence.Repositories;
using InvoiceApp.Domain.Clients;
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
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        
        return services;
    }
}

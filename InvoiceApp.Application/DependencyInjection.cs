using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        // services.AddAuth(configuration)
        //     .AddPersistence(configuration);
        // services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddMediatR(config => 
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });
        
        return services;
    }
}

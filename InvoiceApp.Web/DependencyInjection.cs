using InvoiceApp.Web.Services;

namespace InvoiceApp.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddScoped<IInvoiceFormViewModelFactory, InvoiceFormViewModelFactory>();
        return services;
    }
}
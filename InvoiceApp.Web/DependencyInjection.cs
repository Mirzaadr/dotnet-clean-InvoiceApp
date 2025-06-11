using System.Globalization;
using InvoiceApp.Web.Services;

namespace InvoiceApp.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        // define en-US as culture info, so the app will use dot for decimal
        var cultureInfo = new CultureInfo("en-US");
        cultureInfo.NumberFormat.CurrencySymbol = "IDR "; // use IDR for currency instead of $
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        services.AddControllersWithViews().AddDataAnnotationsLocalization();
        services.AddScoped<IInvoiceFormViewModelFactory, InvoiceFormViewModelFactory>();
        return services;
    }
}
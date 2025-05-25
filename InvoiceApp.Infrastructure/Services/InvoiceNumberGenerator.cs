using InvoiceApp.Application.Commons.Interface;
using InvoiceApp.Domain.Invoices;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.Infrastructure.Services;

public class InvoiceNumberGenerator : IInvoiceNumberGenerator
{
    private const string Prefix = "INV-";
    private const string Suffix = "-2025";
    private int _currentNumber;
    private bool _initialized = false;
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private readonly IServiceProvider _serviceProvider;

    public InvoiceNumberGenerator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<string> GenerateAsync()
    {
        if (!_initialized)
            await InitializeAsync();

        var invoiceNumber = $"{Prefix}{_currentNumber}{Suffix}";
        _currentNumber++;
        return invoiceNumber;
    }

    private async Task InitializeAsync()
    {
        await _initLock.WaitAsync();
        try
        {
            if (_initialized) return;

            // Create a scope to resolve a scoped service from singleton
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IInvoiceRepository>();

            var latest = (await repo.GetLatestAsync()).InvoiceNumber;
            var numPart = latest?.Replace(Prefix, "").Replace(Suffix, "");

            _currentNumber = int.TryParse(numPart, out var n) ? n + 1 : 1000;
            _initialized = true;
        }
        finally
        {
            _initLock.Release();
        }
    }
}

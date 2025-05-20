using InvoiceApp.Application.Commons.Interface;

namespace InvoiceApp.Infrastructure.Services;

public class InvoiceNumberGenerator : IInvoiceNumberGenerator
{
    private const string Prefix = "INV-";
    private const string Suffix = "-2023";
    private int _currentNumber;

    public InvoiceNumberGenerator()
    {
        _currentNumber = 1000; // Starting number for invoice generation
    }

    public string Generate()
    {
        var invoiceNumber = $"{Prefix}{_currentNumber}{Suffix}";
        _currentNumber++;
        return invoiceNumber;
    }
}
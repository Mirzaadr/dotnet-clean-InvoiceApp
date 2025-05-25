namespace InvoiceApp.Application.Commons.Interface;

public interface IInvoiceNumberGenerator
{
    Task<string> GenerateAsync();
}
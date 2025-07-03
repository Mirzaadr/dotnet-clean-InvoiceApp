namespace InvoiceApp.Application.Commons.Interface;

public interface IEmailService
{
    Task SendAsync(string toEmail, string subject, string body, byte[]? attachment = null, string? attachmentName = null);
}

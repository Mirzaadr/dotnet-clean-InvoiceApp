using InvoiceApp.Application.Commons.Interface;
using Microsoft.Extensions.Configuration;

namespace InvoiceApp.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public Task SendAsync(string toEmail, string subject, string body, byte[]? attachment = null, string? attachmentName = null)
    {
        Console.WriteLine($"EMAIL TO: {toEmail}]");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Body: {body}");
        return Task.CompletedTask;
    }
}

using InvoiceApp.Application.Commons.Interface;
using InvoiceApp.Application.Commons.Mappers;
using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.Send;

internal sealed class SendInvoiceDomainEventHandler : INotificationHandler<InvoiceSentNotification>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IPdfService _pdfService;
    private readonly IStorageService _storageService;
    private readonly IEmailService _emailService;

    public SendInvoiceDomainEventHandler(IInvoiceRepository invoiceRepository, IPdfService pdfService, IStorageService storageService, IEmailService emailService)
    {
        _invoiceRepository = invoiceRepository;
        _pdfService = pdfService;
        _storageService = storageService;
        _emailService = emailService;
    }

    public async Task Handle(InvoiceSentNotification notification, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(notification.Event.InvoiceId);
        if (invoice is null)
        {
            throw new Exception("Invoice not found");
        }
        var invoiceDto = InvoiceMapper.ToDto(invoice);

        var pdf = _pdfService.GenerateInvoicePdf(invoiceDto);
        var filePath = await _storageService.SaveFileAsync(invoice.Id.ToString(), pdf, cancellationToken);
        if (filePath is null)
        {
            throw new Exception("Failed to save invoice PDF");
        }

        var body = $"Hello {invoice.ClientName}, your invoice is ready at this link <a href=\"{filePath}\">Download</a>";
        await _emailService.SendAsync(notification.Event.Email, $"Invoice {invoice.InvoiceNumber}", body);
  
        // Console.WriteLine($"Invoice with ID {notification.Event.InvoiceId} has been sent.");
        await Task.CompletedTask;
        return;
    }
}
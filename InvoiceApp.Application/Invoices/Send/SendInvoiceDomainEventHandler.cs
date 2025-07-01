using InvoiceApp.Domain.Commons.Interfaces;
using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.Send;

internal sealed class SendInvoiceDomainEventHandler : INotificationHandler<InvoiceSentNotification>
{
    public Task Handle(InvoiceSentNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Invoice with ID {notification.Event.InvoiceId} has been sent.");
        return Task.CompletedTask;
    }
}
using MediatR;
using InvoiceApp.Domain.Invoices;

namespace InvoiceApp.Application.Invoices.Send;

public class InvoiceSentNotification : INotification
{
    public InvoiceSentEvent Event { get; }

    public InvoiceSentNotification(InvoiceSentEvent e)
    {
        Event = e;
    }
}

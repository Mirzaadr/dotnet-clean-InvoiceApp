using InvoiceApp.Domain.Commons.Interfaces;

namespace InvoiceApp.Domain.Invoices;

public class InvoiceSentEvent : IDomainEvent
{
    public InvoiceId InvoiceId { get; }
    public string Email { get; }
    public string ClientName { get; }
    public string InvoiceNumber { get; }

    public InvoiceSentEvent(InvoiceId invoiceId, string email, string clientName, string invoiceNumber)
    {
        InvoiceId = invoiceId;
        Email = email;
        ClientName = clientName;
        InvoiceNumber = invoiceNumber;
    }
}
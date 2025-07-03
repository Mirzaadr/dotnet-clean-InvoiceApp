using MediatR;

namespace InvoiceApp.Application.Invoices.MarkAsPaid;

public record MarkAsPaidInvoiceCommand(
  Guid InvoiceId
) : IRequest;
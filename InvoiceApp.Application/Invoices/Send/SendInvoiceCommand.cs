using MediatR;

namespace InvoiceApp.Application.Invoices.Send;

public record SendInvoiceCommand(
  Guid InvoiceId
) : IRequest;
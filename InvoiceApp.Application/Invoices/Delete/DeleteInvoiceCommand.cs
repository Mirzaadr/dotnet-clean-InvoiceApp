using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.Delete;

public record DeleteInvoiceCommand(
  Guid InvoiceId
) : IRequest;
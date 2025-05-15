using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.Get;

public record GetInvoiceQuery(InvoiceId InvoiceId) : IRequest<InvoiceResponse>;

public record InvoiceResponse(
  Guid Id,
  Guid ClientId,
  DateTime InvoiceDate,
  DateTime DueDate,
  InvoiceStatusType Status,
  double TotalAmount
  // List<>
);
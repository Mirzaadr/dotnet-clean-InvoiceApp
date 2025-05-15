using MediatR;

namespace InvoiceApp.Application.Invoices.Create;

public record CreateInvoiceCommand(
  Guid clientId,
  DateTime invoiceDate,
  DateTime dueDate,
  string status,
  double total,
  List<InvoiceItemCommand> listItems
) : IRequest;

public record InvoiceItemCommand(
  string name,
  string description,
  int qty,
  double unitPrice
);
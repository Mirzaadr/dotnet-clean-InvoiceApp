using MediatR;

namespace InvoiceApp.Application.Invoices.Create;

public record CreateInvoiceCommand(
  Guid ClientId,
  DateTime IssueDate,
  DateTime DueDate,
  // string Status,
  // double Total,
  List<InvoiceItemCommand> Items
) : IRequest;

public record InvoiceItemCommand(
  Guid ProductId,
  string ProductName,
  int Quantity,
  double UnitPrice
);
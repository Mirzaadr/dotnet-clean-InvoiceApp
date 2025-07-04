using MediatR;

namespace InvoiceApp.Application.Invoices.Create;

public record CreateInvoiceCommand(
    Guid ClientId,
    string InvoiceNumber,
    DateTime IssueDate,
    DateTime DueDate,
    List<InvoiceItemCommand> Items,
    bool IsSend = false
) : IRequest;

public record InvoiceItemCommand(
    Guid ProductId,
    string ProductName,
    int Quantity,
    double UnitPrice
);
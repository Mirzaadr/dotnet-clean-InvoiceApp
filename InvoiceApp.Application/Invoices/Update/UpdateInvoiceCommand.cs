using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Invoices.Update;

public record UpdateInvoiceCommand(
  // InvoiceDTO Invoice,

  Guid InvoiceId,
  DateTime IssueDate,
  DateTime DueDate,
  List<InvoiceItemDto> Items
) : IRequest;
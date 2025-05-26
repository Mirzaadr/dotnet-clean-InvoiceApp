using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Invoices.Update;

public record UpdateInvoiceCommand(
  InvoiceDTO Invoice
) : IRequest;
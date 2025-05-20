using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Invoices.Get;

public record GetInvoicesQuery() : IRequest<List<InvoiceDTO>>;

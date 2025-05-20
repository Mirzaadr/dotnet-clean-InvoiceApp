using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Invoices.GetAll;

public record GetInvoiceListQuery() : IRequest<List<InvoiceDTO>>;

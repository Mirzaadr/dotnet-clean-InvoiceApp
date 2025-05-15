using InvoiceApp.Application.Invoices.Get;
using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.GetAll;

public record GetInvoiceListQuery() : IRequest<List<InvoiceResponse>>;

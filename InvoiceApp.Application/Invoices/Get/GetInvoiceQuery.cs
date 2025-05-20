using InvoiceApp.Application.DTOs;
using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.Get;

public record GetInvoiceQuery(Guid InvoiceId) : IRequest<InvoiceDTO>;
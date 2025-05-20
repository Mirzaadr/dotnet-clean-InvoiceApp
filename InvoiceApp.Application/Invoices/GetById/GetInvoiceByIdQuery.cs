using InvoiceApp.Application.DTOs;
using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.GetById;

public record GetInvoiceByIdQuery(Guid InvoiceId) : IRequest<InvoiceDTO>;
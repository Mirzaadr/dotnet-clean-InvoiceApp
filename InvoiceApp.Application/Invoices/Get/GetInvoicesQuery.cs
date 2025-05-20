using InvoiceApp.Application.DTOs;
using InvoiceApp.Domain.Commons.Models;
using MediatR;

namespace InvoiceApp.Application.Invoices.Get;

public record GetInvoicesQuery(int page, int pageSize, string? searchTerm) : IRequest<PagedList<InvoiceDTO>>;

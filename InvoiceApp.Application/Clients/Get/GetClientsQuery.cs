using InvoiceApp.Application.DTOs;
using InvoiceApp.Domain.Commons.Models;
using MediatR;

namespace InvoiceApp.Application.Clients.Get;

public record GetClientsQuery(
  int page,
  int? pageSize,
  string? searchTerm
) : IRequest<PagedList<ClientDto>>;
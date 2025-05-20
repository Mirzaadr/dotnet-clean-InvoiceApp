using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Clients.Get;

public record GetClientsQuery() : IRequest<List<ClientDto>>;
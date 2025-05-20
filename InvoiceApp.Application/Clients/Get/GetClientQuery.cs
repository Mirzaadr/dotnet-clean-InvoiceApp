using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Clients.Get;

public record GetClientQuery(Guid ClientId) : IRequest<ClientDto>;
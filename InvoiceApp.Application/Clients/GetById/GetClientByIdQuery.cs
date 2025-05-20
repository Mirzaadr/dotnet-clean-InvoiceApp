using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Clients.GetById;

public record GetClientByIdQuery(Guid ClientId) : IRequest<ClientDto>;
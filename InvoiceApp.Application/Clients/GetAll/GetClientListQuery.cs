using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Clients.GetAll;

public record GetClientListQuery() : IRequest<List<ClientDto>>;
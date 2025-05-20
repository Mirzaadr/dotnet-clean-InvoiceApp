using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Clients.Create;

public record CreateClientCommand(
  ClientDto client
) : IRequest;

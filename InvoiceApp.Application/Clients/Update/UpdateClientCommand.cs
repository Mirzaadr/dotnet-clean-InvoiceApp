using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Clients.Update;

public record UpdateClientCommand(
  ClientDto client
) : IRequest;

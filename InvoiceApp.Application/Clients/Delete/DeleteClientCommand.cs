using MediatR;

namespace InvoiceApp.Application.Clients.Delete;

public record DeleteClientCommand(
  Guid ClientId
) : IRequest;
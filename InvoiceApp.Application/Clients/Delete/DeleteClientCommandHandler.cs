using InvoiceApp.Domain.Clients;
using MediatR;

namespace InvoiceApp.Application.Clients.Delete;

internal class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand>
{
  private readonly IClientRepository _clientRepository;

  public DeleteClientCommandHandler(IClientRepository clientRepository)
  {
      _clientRepository = clientRepository;
  }

  public async Task Handle(DeleteClientCommand command, CancellationToken cancellationToken)
  {
      var client = await _clientRepository.GetByIdAsync(ClientId.FromGuid(command.ClientId));
      if (client is null) throw new Exception("Client not found");

      await _clientRepository.DeleteAsync(client);
  }
}
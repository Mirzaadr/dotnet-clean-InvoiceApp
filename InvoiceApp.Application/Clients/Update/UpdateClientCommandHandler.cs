using InvoiceApp.Domain.Clients;
using MediatR;

namespace InvoiceApp.Application.Clients.Update;

internal class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand>
{
    private readonly IClientRepository _clientRepository;

  public UpdateClientCommandHandler(IClientRepository clientRepository)
  {
      _clientRepository = clientRepository;
  }

  public async Task Handle(UpdateClientCommand command, CancellationToken cancellationToken)
  {
    await _clientRepository.UpdateAsync(new Client(
      new ClientId(command.client.Id),
      command.client.Name,
      command.client.Address,
      command.client.Email,
      command.client.PhoneNumber,
      null,
      null
    ));
    await Task.CompletedTask;
  }
}
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
    var client = await _clientRepository.GetByIdAsync(ClientId.FromGuid(command.Id));
    if (client is null)
    {
      throw new Exception("not found");
    }

    client.Update(
      command.Name,
      command.Address,
      command.Email,
      command.PhoneNumber,
      null
    );

    await _clientRepository.UpdateAsync(client);
    await Task.CompletedTask;
  }
}
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
      new ClientId(command.Id),
      command.Name,
      command.Address,
      command.Email,
      command.PhoneNumber,
      null,
      null
    ));
    await Task.CompletedTask;
  }
}
using InvoiceApp.Domain.Clients;
using MediatR;

namespace InvoiceApp.Application.Clients.Create;

internal class CreateClientCommandHandler : IRequestHandler<CreateClientCommand>
{
  private readonly IClientRepository _clientRepository;

  public CreateClientCommandHandler(IClientRepository clientRepository)
  {
    _clientRepository = clientRepository;
  }

  public async Task Handle(CreateClientCommand command, CancellationToken cancellationToken)
  {
    // TODO: check by name in database
    await Task.CompletedTask;
    var newClient = Client.Create(
      command.Name,
      command.Address,
      command.Email,
      command.PhoneNumber
    );

    await _clientRepository.AddAsync(newClient);
  }
}
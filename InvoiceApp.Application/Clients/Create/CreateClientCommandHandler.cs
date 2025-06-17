using InvoiceApp.Domain.Clients;
using MediatR;

namespace InvoiceApp.Application.Clients.Create;

internal class CreateClientCommandHandler : IRequestHandler<CreateClientCommand>
{
  private readonly IClientRepository _clientRepository;
  private readonly IUnitOfWork _unitOfWork;

  public CreateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
  {
    _clientRepository = clientRepository;
    _unitOfWork = unitOfWork;
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
    await _unitOfWork.SaveChangesAsync(cancellationToken);
  }
}
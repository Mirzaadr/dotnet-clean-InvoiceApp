using InvoiceApp.Domain.Clients;
using MediatR;

namespace InvoiceApp.Application.Clients.Update;

internal class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

  public UpdateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
  {
      _clientRepository = clientRepository;
      _unitOfWork = unitOfWork;
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
    await _unitOfWork.SaveChangesAsync(cancellationToken);

  }
}
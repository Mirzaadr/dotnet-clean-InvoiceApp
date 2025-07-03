using InvoiceApp.Domain.Clients;
using MediatR;

namespace InvoiceApp.Application.Clients.Delete;

internal class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand>
{
  private readonly IClientRepository _clientRepository;
  private readonly IUnitOfWork _unitOfWork;

  public DeleteClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
  {
      _clientRepository = clientRepository;
      _unitOfWork = unitOfWork;
  }

  public async Task Handle(DeleteClientCommand command, CancellationToken cancellationToken)
  {
      var client = await _clientRepository.GetByIdAsync(ClientId.FromGuid(command.ClientId));
      if (client is null) throw new Exception("Client not found");

      await _clientRepository.DeleteAsync(client);
      await _unitOfWork.SaveChangesAsync(cancellationToken);
  }
}
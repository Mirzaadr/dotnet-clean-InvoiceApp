using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Mappers;
using InvoiceApp.Domain.Clients;
using MediatR;

namespace InvoiceApp.Application.Clients.GetAll;

internal sealed class GetClientListQueryHandler : IRequestHandler<GetClientListQuery, List<ClientDto>>
{
    private readonly IClientRepository _clientRepository;

    public GetClientListQueryHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<List<ClientDto>> Handle(GetClientListQuery request, CancellationToken cancellationToken)
    {
        var clients = await _clientRepository.GetAllAsync();
        return clients.ConvertAll(client =>
          ClientMapper.ToDto(client));
    }
}
using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Commons.Mappers;
using InvoiceApp.Domain.Clients;
using MediatR;

namespace InvoiceApp.Application.Clients.Get;

internal sealed class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, List<ClientDto>>
{
    private readonly IClientRepository _clientRepository;

    public GetClientsQueryHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<List<ClientDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await _clientRepository.GetAllAsync();
        return clients.ConvertAll(client =>
          ClientMapper.ToDto(client));
    }
}
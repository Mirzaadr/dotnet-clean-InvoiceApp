using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Mappers;
using InvoiceApp.Domain.Clients;
using MediatR;

namespace InvoiceApp.Application.Clients.Get;

internal sealed class GetClientQueryHandler : IRequestHandler<GetClientQuery, ClientDto>
{
    private readonly IClientRepository _clientRepository;

    public GetClientQueryHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ClientDto> Handle(GetClientQuery request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(new ClientId(request.ClientId));
        if (client is null)
        {
            throw new Exception("client not found");
        }

        return ClientMapper.ToDto(client);
    }
}

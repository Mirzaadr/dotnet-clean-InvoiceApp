using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Commons.Mappers;
using InvoiceApp.Domain.Clients;
using MediatR;

namespace InvoiceApp.Application.Clients.GetById;

internal sealed class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientDto>
{
    private readonly IClientRepository _clientRepository;

    public GetClientByIdQueryHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ClientDto> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(ClientId.FromGuid(request.ClientId));
        if (client is null)
        {
            throw new Exception("client not found");
        }

        return ClientMapper.ToDto(client);
    }
}

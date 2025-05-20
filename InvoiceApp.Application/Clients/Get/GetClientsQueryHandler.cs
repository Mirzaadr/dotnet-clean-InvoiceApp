using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Commons.Mappers;
using InvoiceApp.Domain.Clients;
using MediatR;
using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Application.Clients.Get;

internal sealed class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, PagedList<ClientDto>>
{
    private readonly IClientRepository _clientRepository;

    public GetClientsQueryHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<PagedList<ClientDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await _clientRepository.GetAllAsync(
            request.page,
            request.pageSize,
            request.searchTerm
        );
        return new PagedList<ClientDto>(
            items: clients.Items.ConvertAll(client => ClientMapper.ToDto(client)),
            page: clients.Page,
            pageSize: clients.PageSize,
            totalCount: clients.TotalCount
        );
    }
}
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
        if (request.pageSize is null)
        {
            var clients = await _clientRepository.GetAllAsync();
            return new PagedList<ClientDto>(
                items: clients.ConvertAll(client => ClientMapper.ToDto(client)),
                page: request.page,
                pageSize: clients.Count(),
                totalCount: clients.Count()
            );
        }
        else
        {
            var clients = await _clientRepository.GetAllAsync(
                request.page,
                request.pageSize.GetValueOrDefault(10),
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
}
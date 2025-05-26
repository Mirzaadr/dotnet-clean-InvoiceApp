using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Commons.Mappers;
using InvoiceApp.Domain.Invoices;
using MediatR;
using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Application.Invoices.Get;

internal sealed class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, PagedList<InvoiceDTO>>
{
  private readonly IInvoiceRepository _invoiceRepository;

  public GetInvoicesQueryHandler(IInvoiceRepository invoiceRepository)
  {
      _invoiceRepository = invoiceRepository;
  }

  public async Task<PagedList<InvoiceDTO>> Handle(GetInvoicesQuery query, CancellationToken cancellationToken)
  {
    var invoices = await _invoiceRepository.GetAllAsync(query.page, query.pageSize, query.searchTerm);
    return new PagedList<InvoiceDTO>(
      items: invoices.Items.ConvertAll(i => InvoiceMapper.ToDto(i)),
      page: invoices.Page,
      pageSize: invoices.PageSize,
      totalCount: invoices.TotalCount
    );
  }
}
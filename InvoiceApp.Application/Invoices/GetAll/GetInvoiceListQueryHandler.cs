using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Invoices.Get;
using InvoiceApp.Application.Mappers;
using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.GetAll;

internal sealed class GetInvoiceListQueryHandler : IRequestHandler<GetInvoiceListQuery, List<InvoiceDTO>>
{
  private readonly IInvoiceRepository _invoiceRepository;

  public GetInvoiceListQueryHandler(IInvoiceRepository invoiceRepository)
  {
      _invoiceRepository = invoiceRepository;
  }

  public async Task<List<InvoiceDTO>> Handle(GetInvoiceListQuery query, CancellationToken cancellationToken)
  {
    var invoices = await _invoiceRepository.GetAllAsync();
    return invoices.ConvertAll(invoice => 
      InvoiceMapper.ToDto(invoice)
    );
  }
}
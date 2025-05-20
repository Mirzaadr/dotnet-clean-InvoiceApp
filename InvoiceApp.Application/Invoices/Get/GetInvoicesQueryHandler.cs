using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Commons.Mappers;
using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.Get;

internal sealed class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, List<InvoiceDTO>>
{
  private readonly IInvoiceRepository _invoiceRepository;

  public GetInvoicesQueryHandler(IInvoiceRepository invoiceRepository)
  {
      _invoiceRepository = invoiceRepository;
  }

  public async Task<List<InvoiceDTO>> Handle(GetInvoicesQuery query, CancellationToken cancellationToken)
  {
    var invoices = await _invoiceRepository.GetAllAsync();
    return invoices.ConvertAll(invoice => 
      InvoiceMapper.ToDto(invoice)
    );
  }
}
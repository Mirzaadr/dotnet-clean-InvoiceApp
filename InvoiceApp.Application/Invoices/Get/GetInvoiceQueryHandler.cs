using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Mappers;
using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.Get;

internal sealed class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, InvoiceDTO>
{
  private readonly IInvoiceRepository _invoiceRepository;

  public GetInvoiceQueryHandler(IInvoiceRepository invoiceRepository)
  {
    _invoiceRepository = invoiceRepository;
  }

  public async Task<InvoiceDTO> Handle(GetInvoiceQuery query, CancellationToken cancellationToken)
  {
    var invoice = await _invoiceRepository.GetByIdAsync(new InvoiceId(query.InvoiceId));
    // throw new NotImplementedException();
    if (invoice is null)
    {
        throw new Exception("Invoice not found");
    }
    return InvoiceMapper.ToDto(invoice);
  }
}
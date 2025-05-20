using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Commons.Mappers;
using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.GetById;

internal sealed class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDTO>
{
  private readonly IInvoiceRepository _invoiceRepository;

  public GetInvoiceQueryHandler(IInvoiceRepository invoiceRepository)
  {
    _invoiceRepository = invoiceRepository;
  }

  public async Task<InvoiceDTO> Handle(GetInvoiceByIdQuery query, CancellationToken cancellationToken)
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
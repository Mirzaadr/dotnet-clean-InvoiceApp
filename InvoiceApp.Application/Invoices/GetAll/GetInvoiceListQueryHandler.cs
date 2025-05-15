using InvoiceApp.Application.Invoices.Get;
using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.GetAll;

internal sealed class GetInvoiceListQueryHandler : IRequestHandler<GetInvoiceListQuery, List<InvoiceResponse>>
{
  private readonly IInvoiceRepository _invoiceRepository;

  public GetInvoiceListQueryHandler(IInvoiceRepository invoiceRepository)
  {
      _invoiceRepository = invoiceRepository;
  }

  public async Task<List<InvoiceResponse>> Handle(GetInvoiceListQuery request, CancellationToken cancellationToken)
  {
    var invoices = await _invoiceRepository.GetAll();
    return invoices.ConvertAll(invoice => 
      new InvoiceResponse(
        invoice.Id.Value,
        invoice.ClientId.Value,
        invoice.InvoiceDate,
        invoice.DueDate,
        invoice.InvoiceStatus.Status,
        invoice.TotalAmount
      )
    );
  }
}
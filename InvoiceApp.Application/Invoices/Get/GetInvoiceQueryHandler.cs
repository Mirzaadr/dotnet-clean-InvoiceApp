using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.Get;

internal sealed class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, InvoiceResponse>
{
  private readonly IInvoiceRepository _invoiceRepository;

  public GetInvoiceQueryHandler(IInvoiceRepository invoiceRepository)
  {
    _invoiceRepository = invoiceRepository;
  }

  public async Task<InvoiceResponse> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
  {
    var invoice = await _invoiceRepository.GetById(request.InvoiceId);
    // throw new NotImplementedException();
    if (invoice is null)
    {
        throw new Exception("Invoice not found");
    }
    return new InvoiceResponse(
      invoice.Id.Value,
      invoice.ClientId.Value,
      invoice.InvoiceDate,
      invoice.DueDate,
      invoice.InvoiceStatus.Status,
      invoice.TotalAmount
    );
  }
}
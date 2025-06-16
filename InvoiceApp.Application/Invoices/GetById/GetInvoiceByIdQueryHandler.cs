using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Commons.Mappers;
using InvoiceApp.Domain.Invoices;
using MediatR;
using InvoiceApp.Domain.Clients;

namespace InvoiceApp.Application.Invoices.GetById;

internal sealed class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceDTO>
{
  private readonly IInvoiceRepository _invoiceRepository;
  private readonly IClientRepository _clientRepository;

  public GetInvoiceQueryHandler(IInvoiceRepository invoiceRepository, IClientRepository clientRepository)
  {
    _invoiceRepository = invoiceRepository;
    _clientRepository = clientRepository;
  }

  public async Task<InvoiceDTO> Handle(GetInvoiceByIdQuery query, CancellationToken cancellationToken)
  {
    var invoice = await _invoiceRepository.GetByIdAsync(InvoiceId.FromGuid(query.InvoiceId));
    // throw new NotImplementedException();
    if (invoice is null)
    {
        throw new Exception("Invoice not found");
    }
    InvoiceDTO invoiceDTO = InvoiceMapper.ToDto(invoice);

    var client = await _clientRepository.GetByIdAsync(invoice.ClientId);

    invoiceDTO.ClientAddress = client?.Address ;
    invoiceDTO.ClientEmail = client?.Email;
    invoiceDTO.ClientPhone = client?.PhoneNumber;
    return invoiceDTO;
  }
}
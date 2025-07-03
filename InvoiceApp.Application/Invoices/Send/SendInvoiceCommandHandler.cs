using System.ComponentModel.DataAnnotations;
using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.Send;

internal class SendInvoiceCommandHandler : IRequestHandler<SendInvoiceCommand>
{
  private readonly IInvoiceRepository _invoiceRepository;
  private readonly IClientRepository _clientRepository;
  private readonly IUnitOfWork _unitOfWork;

  public SendInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork, IClientRepository clientRepository)
  {
    _invoiceRepository = invoiceRepository;
    _unitOfWork = unitOfWork;
    _clientRepository = clientRepository;
  }

  public async Task Handle(SendInvoiceCommand request, CancellationToken cancellationToken)
  {
    var invoice = await _invoiceRepository.GetByIdAsync(InvoiceId.FromGuid(request.InvoiceId));
    if (invoice is null)
    {
      throw new ValidationException("invoice with id does not exist");
    }

    var client = await _clientRepository.GetByIdAsync(invoice.ClientId);
    if (client is null)
    {
      throw new ValidationException("Client not found");
    }
    invoice.MarkAsSent();

    invoice.Raise(new InvoiceSentEvent(invoice.Id, client.Email ?? "", invoice.ClientName, invoice.InvoiceNumber));

    // save
    await _unitOfWork.SaveChangesAsync(cancellationToken);
    return;
  }
}
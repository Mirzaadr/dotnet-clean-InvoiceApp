using System.ComponentModel.DataAnnotations;
using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.Send;

internal class SendInvoiceCommandHandler : IRequestHandler<SendInvoiceCommand>
{
  private readonly IInvoiceRepository _invoiceRepository;
  private readonly IUnitOfWork _unitOfWork;

  public SendInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork)
  {
    _invoiceRepository = invoiceRepository;
    _unitOfWork = unitOfWork;
  }

  public async Task Handle(SendInvoiceCommand request, CancellationToken cancellationToken)
  {
    var invoice = await _invoiceRepository.GetByIdAsync(InvoiceId.FromGuid(request.InvoiceId));
    if (invoice is null)
    {
      throw new ValidationException("invoice with id does not exist");
    }
    invoice.MarkAsSent();

    // save
    await _unitOfWork.SaveChangesAsync(cancellationToken);
    return;
  }
}
using System.ComponentModel.DataAnnotations;
using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.MarkAsPaid;

internal class MarkAsPaidInvoiceCommandHandler : IRequestHandler<MarkAsPaidInvoiceCommand>
{
  private readonly IInvoiceRepository _invoiceRepository;
  private readonly IUnitOfWork _unitOfWork;

  public MarkAsPaidInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork)
  {
    _invoiceRepository = invoiceRepository;
    _unitOfWork = unitOfWork;
  }

  public async Task Handle(MarkAsPaidInvoiceCommand request, CancellationToken cancellationToken)
  {
    var invoice = await _invoiceRepository.GetByIdAsync(InvoiceId.FromGuid(request.InvoiceId));
    if (invoice is null)
    {
      throw new ValidationException("invoice with id does not exist");
    }
    invoice.MarkAsPaid();

    // save
    await _unitOfWork.SaveChangesAsync(cancellationToken);
    return;
  }
}
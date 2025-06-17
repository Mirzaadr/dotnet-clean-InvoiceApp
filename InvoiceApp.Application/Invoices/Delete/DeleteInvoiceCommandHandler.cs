using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.Delete;

internal class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand>
{
  private readonly IInvoiceRepository _invoiceRepository;
  private readonly IUnitOfWork _unitOfWork;

  public DeleteInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork)
  {
      _invoiceRepository = invoiceRepository;
      _unitOfWork = unitOfWork;
  }

  public async Task Handle(DeleteInvoiceCommand command, CancellationToken cancellationToken)
  {
      var invoice = await _invoiceRepository.GetByIdAsync(InvoiceId.FromGuid(command.InvoiceId));
      if (invoice is null) throw new Exception("Invoice not found");

      await _invoiceRepository.DeleteAsync(invoice);
      await _unitOfWork.SaveChangesAsync(cancellationToken);
  }
}
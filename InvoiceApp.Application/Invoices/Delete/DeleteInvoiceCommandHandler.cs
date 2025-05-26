using InvoiceApp.Domain.Invoices;
using MediatR;

namespace InvoiceApp.Application.Invoices.Delete;

internal class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand>
{
  private readonly IInvoiceRepository _invoiceRepository;

  public DeleteInvoiceCommandHandler(IInvoiceRepository invoiceRepository)
  {
      _invoiceRepository = invoiceRepository;
  }

  public async Task Handle(DeleteInvoiceCommand command, CancellationToken cancellationToken)
  {
      var invoice = await _invoiceRepository.GetByIdAsync(new InvoiceId(command.InvoiceId));
      if (invoice is null) throw new Exception("Invoice not found");

      await _invoiceRepository.DeleteAsync(invoice);
  }
}
using InvoiceApp.Application.Commons.Interface;
using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Invoices;
using InvoiceApp.Domain.Products;
using MediatR;

namespace InvoiceApp.Application.Invoices.Update;

internal class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand>
{
    private readonly IClientRepository _clientRepository;
    private readonly IInvoiceRepository _invoiceRepository;

  public UpdateInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IClientRepository clientRepository)
  {
      _invoiceRepository = invoiceRepository;
      _clientRepository = clientRepository;
  }

  public async Task Handle(UpdateInvoiceCommand command, CancellationToken cancellationToken)
  {
    await Task.CompletedTask;

    var currentInvoice = await _invoiceRepository.GetByIdAsync(new InvoiceId(command.InvoiceId));
    if (currentInvoice is null) throw new Exception("Invoice not found");

    if (currentInvoice.Status != InvoiceStatus.Draft)
      throw new Exception("Only draft invoices can be updated");

    currentInvoice.UpdateInvoiceDates(
      issueDate: command.IssueDate,
      dueDate: command.DueDate
    );

    currentInvoice.UpdateItems(command.Items.ConvertAll(item => new InvoiceItem(
      id: item.Id == Guid.Empty ? InvoiceItemId.New() : new InvoiceItemId(item.Id),
      productId: new ProductId(item.ProductId),
      productName: item.ProductName,
      quantity: item.Quantity,
      unitPrice: item.UnitPrice,
      createdTime: item.CreatedDate,
      updatedTime: item.UpdatedDate
    )));

    await _invoiceRepository.UpdateAsync(currentInvoice);
  }
}
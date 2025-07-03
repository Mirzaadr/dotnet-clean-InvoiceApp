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
    private readonly IUnitOfWork _unitOfWork;

  public UpdateInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IClientRepository clientRepository, IUnitOfWork unitOfWork)
  {
      _invoiceRepository = invoiceRepository;
      _clientRepository = clientRepository;
      _unitOfWork = unitOfWork;
  }

  public async Task Handle(UpdateInvoiceCommand command, CancellationToken cancellationToken)
  {
    var currentInvoice = await _invoiceRepository.GetByIdAsync(InvoiceId.FromGuid(command.InvoiceId));
    if (currentInvoice is null) throw new Exception("Invoice not found");

    if (currentInvoice.Status != InvoiceStatus.Draft)
      throw new Exception("Only draft invoices can be updated");

    currentInvoice.UpdateInvoiceDates(
      issueDate: command.IssueDate,
      dueDate: command.DueDate
    );

    currentInvoice.UpdateItems(command.Items.ConvertAll(item => new InvoiceItem(
      id: item.Id == Guid.Empty ? InvoiceItemId.New() : InvoiceItemId.FromGuid(item.Id),
      productId: ProductId.FromGuid(item.ProductId),
      productName: item.ProductName,
      quantity: item.Quantity,
      unitPrice: item.UnitPrice,
      createdTime: item.CreatedDate,
      updatedTime: item.UpdatedDate
    )));

    if (command.IsSend)
    {
      var client = await _clientRepository.GetByIdAsync(currentInvoice.ClientId);
      if (client is null) throw new Exception("Client not found");

      currentInvoice.MarkAsSent();

      currentInvoice.Raise(new InvoiceSentEvent(currentInvoice.Id, client.Email ?? "", currentInvoice.ClientName, currentInvoice.InvoiceNumber));

    }

    await _invoiceRepository.UpdateAsync(currentInvoice);
    await _unitOfWork.SaveChangesAsync(cancellationToken);
  }
}
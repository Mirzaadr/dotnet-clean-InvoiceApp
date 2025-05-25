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
    var invoice = new Invoice(
      // request.clientId,
      id: new InvoiceId(command.Invoice.Id),
      invoiceNum: command.Invoice.InvoiceNumber,
      clientId: new ClientId(command.Invoice.ClientId),
      clientName: command.Invoice.ClientName,
      issueDate: command.Invoice.IssueDate,
      dueDate: command.Invoice.DueDate,
      status: InvoiceStatus.From(command.Invoice.Status),
      createdDate: command.Invoice.CreatedDate.GetValueOrDefault(),
      updatedDate: command.Invoice.UpdatedDate.GetValueOrDefault(),
      listItems: command.Invoice.Items.ConvertAll(item => new InvoiceItem(
        id: item.Id == Guid.Empty ? InvoiceItemId.New() : new InvoiceItemId(item.Id),
        productId: new ProductId(item.ProductId),
        productName: item.ProductName,
        quantity: item.Quantity,
        unitPrice: item.UnitPrice,
        createdTime: item.CreatedDate,
        updatedTime: item.UpdatedDate
      ))
    );

    await _invoiceRepository.UpdateAsync(invoice);
  }
}
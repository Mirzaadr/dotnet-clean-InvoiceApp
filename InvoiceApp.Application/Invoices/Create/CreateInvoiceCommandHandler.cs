using System.ComponentModel.DataAnnotations;
using InvoiceApp.Application.Commons.Interface;
using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Invoices;
using InvoiceApp.Domain.Products;
using MediatR;

namespace InvoiceApp.Application.Invoices.Create;

internal class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand>
{
    private readonly IClientRepository _clientRepository;
  private readonly IInvoiceRepository _invoiceRepository;

  public CreateInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IClientRepository clientRepository)
  {
      _invoiceRepository = invoiceRepository;
      _clientRepository = clientRepository;
  }

  public async Task Handle(CreateInvoiceCommand command, CancellationToken cancellationToken)
  {
    var client = await _clientRepository.GetByIdAsync(new ClientId(command.ClientId));
    if (client is null)
    {
        throw new ValidationException($"Client with ID {command.ClientId} does not exist.");
    }
    await Task.CompletedTask;
    var invoice = Invoice.Create(
      invoiceNum: command.InvoiceNumber,
      clientId: client.Id,
      clientName: client.Name,
      issueDate: command.IssueDate,
      dueDate: command.DueDate,
      status: InvoiceStatus.Draft,
      items: command.Items.ConvertAll(item => new InvoiceItem(
        id: InvoiceItemId.New(),
        productId: new ProductId(item.ProductId),
        productName: item.ProductName,
        quantity: item.Quantity,
        unitPrice: item.UnitPrice,
        createdTime: null,
        updatedTime: null
      ))
    );

    await _invoiceRepository.AddAsync(invoice);
  }
}
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
    private readonly IUnitOfWork _unitOfWork;

    public CreateInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IClientRepository clientRepository, IUnitOfWork unitOfWork)
    {
      _invoiceRepository = invoiceRepository;
      _clientRepository = clientRepository;
      _unitOfWork = unitOfWork;
    }

  public async Task Handle(CreateInvoiceCommand command, CancellationToken cancellationToken)
  {
    var client = await _clientRepository.GetByIdAsync(ClientId.FromGuid(command.ClientId));
    if (client is null)
    {
        throw new ValidationException($"Client with ID {command.ClientId} does not exist.");
    }
    await Task.CompletedTask;
    var invoice = Invoice.Create(
      invoiceNum: command.InvoiceNumber,
      clientId: client.Id,
      clientName: client.Name,
      issueDate: DateTime.SpecifyKind(command.IssueDate, DateTimeKind.Utc),
      dueDate: DateTime.SpecifyKind(command.DueDate, DateTimeKind.Utc),
      status: InvoiceStatus.Draft,
      items: command.Items.ConvertAll(item => InvoiceItem.Create(
        productId: ProductId.FromGuid(item.ProductId),
        productName: item.ProductName,
        quantity: item.Quantity,
        unitPrice: item.UnitPrice
      ))
    );

    await _invoiceRepository.AddAsync(invoice);
    await _unitOfWork.SaveChangesAsync(cancellationToken);
  }
}
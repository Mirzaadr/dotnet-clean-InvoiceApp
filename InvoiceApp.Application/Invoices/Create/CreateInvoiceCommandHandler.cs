using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Invoices;
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

    public async Task Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetById(new ClientId(request.clientId));
        if (client is null)
        {
            return;
        }
        await Task.CompletedTask;
        var invoice = Invoice.Create(
          // request.clientId,
          client.Id,
          request.invoiceDate,
          request.dueDate,
          new InvoiceStatus(InvoiceStatusType.Pending),
          request.listItems.ConvertAll(item => LineItem.Create(
            item.name,
            item.description,
            item.qty,
            item.unitPrice
          ))
        );
        _invoiceRepository.Create(invoice);
    }
}
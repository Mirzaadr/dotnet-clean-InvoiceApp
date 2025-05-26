using InvoiceApp.Application.Clients.Get;
using InvoiceApp.Application.Commons.Interface;
using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Products.Get;
using InvoiceApp.Web.Models.ViewModels;
using MediatR;

namespace InvoiceApp.Web.Services;

public class InvoiceFormViewModelFactory : IInvoiceFormViewModelFactory
{
    private readonly ISender _mediator;
    public InvoiceFormViewModelFactory(ISender mediator)
    {
        _mediator = mediator;
    }

    public async Task<InvoiceFormViewModel> CreateAsync(InvoiceDTO invoice, CancellationToken cancellationToken = default)
    {
        var clientsTask = _mediator.Send(new GetClientsQuery(1, null, null), cancellationToken);
        var productsTask = _mediator.Send(new GetProductsQuery(1, null, null), cancellationToken);

        await Task.WhenAll(clientsTask, productsTask);

        return new InvoiceFormViewModel
        {
            Invoice = invoice,
            Clients = clientsTask.Result.Items,
            Products = productsTask.Result.Items
        };
    }
}
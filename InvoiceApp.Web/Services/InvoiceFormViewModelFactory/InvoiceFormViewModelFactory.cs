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
    private readonly ICacheService _cache;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    public InvoiceFormViewModelFactory(ISender mediator, ICacheService cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    public async Task<InvoiceFormViewModel> CreateAsync(InvoiceDTO invoice, CancellationToken cancellationToken = default)
    {
        var clientsTask = _cache.GetOrCreateAsync("clients_cache", 
            () => _mediator.Send(new GetClientsQuery(1, null, null), cancellationToken), CacheDuration);

        var productsTask = _cache.GetOrCreateAsync("products_cache", 
            () => _mediator.Send(new GetProductsQuery(1, null, null), cancellationToken), CacheDuration);

        await Task.WhenAll(clientsTask, productsTask);

        return new InvoiceFormViewModel
        {
            Invoice = invoice,
            Clients = clientsTask.Result.Items,
            Products = productsTask.Result.Items
        };
    }
}
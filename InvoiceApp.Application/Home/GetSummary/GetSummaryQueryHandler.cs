using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Commons.Mappers;
using InvoiceApp.Domain.Invoices;
using MediatR;
using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Products;

namespace InvoiceApp.Application.Home.GetSummary;

internal sealed class GetSummaryQueryHandler : IRequestHandler<GetSummaryQuery, SummaryDto>
{
  private readonly IInvoiceRepository _invoiceRepository;
  private readonly IClientRepository _clientRepository;
  private readonly IProductRepository _productRepository;

  public GetSummaryQueryHandler(
    IInvoiceRepository invoiceRepository,
    IClientRepository clientRepository,
    IProductRepository productRepository)
  {
    _invoiceRepository = invoiceRepository;
    _clientRepository = clientRepository;
    _productRepository = productRepository;
  }

  public async Task<SummaryDto> Handle(GetSummaryQuery query, CancellationToken cancellationToken)
  {
    var invoiceTask = _invoiceRepository.GetAllAsync(1, 5, null, "createdDate", "desc");
    var invoiceAmountTask = _invoiceRepository.GetTotalAmountAsync();
    var clientTask = _clientRepository.GetAllAsync();
    var productTask = _productRepository.GetAllAsync();

    await Task.WhenAll(
      invoiceTask,
      clientTask,
      invoiceAmountTask,
      productTask);

    var mostExpensive = productTask.Result.OrderByDescending(p => p.UnitPrice).FirstOrDefault();
    var cheapest = productTask.Result.OrderBy(p => p.UnitPrice).FirstOrDefault();

    return new SummaryDto
    {
      TotalInvoices = invoiceTask.Result.TotalCount,
      TotalInvoicedAmount = invoiceAmountTask.Result,
      TotalClients = clientTask.Result.Count(),
      TotalProducts = productTask.Result.Count(),
      ProductMostExpensive = mostExpensive?.Name ?? "N/A",
      ProductMaxPrice = mostExpensive?.UnitPrice ?? 0,
      ProductCheapest = cheapest?.Name ?? "N/A",
      ProductMinPrice = cheapest?.UnitPrice ?? 0,
      RecentInvoices = invoiceTask.Result.Items.ConvertAll(i => InvoiceMapper.ToDto(i))
    };
  }
}
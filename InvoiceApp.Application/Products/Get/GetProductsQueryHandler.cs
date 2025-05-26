using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Commons.Mappers;
using InvoiceApp.Domain.Products;
using MediatR;
using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Application.Products.Get;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedList<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PagedList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        if (request.pageSize is null)
        {
            var products = await _productRepository.GetAllAsync();
            return new PagedList<ProductDto>(
                items: products.ConvertAll(product => ProductMapper.ToDto(product)),
                page: request.page,
                pageSize: products.Count(),
                totalCount: products.Count()
            );
        }
        else
        {
            var products = await _productRepository.GetAllAsync(
                request.page,
                request.pageSize.GetValueOrDefault(10),
                request.searchTerm
            );
            return new PagedList<ProductDto>(
                items: products.Items.ConvertAll(product => ProductMapper.ToDto(product)),
                page: products.Page,
                pageSize: products.PageSize,
                totalCount: products.TotalCount
            );
        }
    }
}
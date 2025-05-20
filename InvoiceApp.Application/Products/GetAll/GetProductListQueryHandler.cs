using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Mappers;
using InvoiceApp.Domain.Products;
using MediatR;

namespace InvoiceApp.Application.Products.GetAll;

public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, List<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetProductListQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductDto>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync();
        return products.ConvertAll(product =>
          ProductMapper.ToDto(product));
    }
}
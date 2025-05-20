using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Mappers;
using InvoiceApp.Domain.Products;
using MediatR;

namespace InvoiceApp.Application.Products.Get;

internal sealed class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public GetProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(new ProductId(request.ProductId));
        if (product is null)
        {
            throw new Exception("Product not found");
        }

        return ProductMapper.ToDto(product);
    }
}

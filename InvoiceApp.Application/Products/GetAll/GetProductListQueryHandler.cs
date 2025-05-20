using InvoiceApp.Application.DTOs;
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
          new ProductDto
          {
              Id = product.Id.Value,
              Name = product.Name,
              Description = product.Description,
              Price = product.UnitPrice,
              CreatedDate = product.CreatedDate,
              UpdatedDate = product.UpdatedDate,
          });
    }
}
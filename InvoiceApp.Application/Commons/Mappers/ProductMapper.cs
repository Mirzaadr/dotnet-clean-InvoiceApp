using InvoiceApp.Application.DTOs;
using InvoiceApp.Domain.Products;

namespace InvoiceApp.Application.Commons.Mappers;

public static class ProductMapper
{
  public static ProductDto ToDto(Product product)
  {
    return new ProductDto
    {
      Id = product.Id.Value,
      Name = product.Name,
      Description = product.Description,
      Price = product.UnitPrice,
      CreatedDate = product.CreatedDate,
      UpdatedDate = product.UpdatedDate,
    };
  }
}
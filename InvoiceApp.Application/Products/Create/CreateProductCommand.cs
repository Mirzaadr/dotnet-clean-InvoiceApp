using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Products.Create;

public record CreateProductCommand(
  ProductDto product
) : IRequest;

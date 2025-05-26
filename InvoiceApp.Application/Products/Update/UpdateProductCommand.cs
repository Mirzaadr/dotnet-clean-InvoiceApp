using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Products.Update;

public record UpdateProductCommand(
  ProductDto product
) : IRequest;

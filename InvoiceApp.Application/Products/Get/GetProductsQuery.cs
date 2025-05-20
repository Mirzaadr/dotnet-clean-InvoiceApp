using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Products.Get;

public record GetProductsQuery() : IRequest<List<ProductDto>>;
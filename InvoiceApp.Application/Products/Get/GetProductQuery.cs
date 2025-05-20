using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Products.Get;

public record GetProductQuery(Guid ProductId) : IRequest<ProductDto>;
using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Products.GetById;

public record GetProductByIdQuery(Guid ProductId) : IRequest<ProductDto>;
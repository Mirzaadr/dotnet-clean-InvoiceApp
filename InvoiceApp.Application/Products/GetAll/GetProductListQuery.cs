using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Products.GetAll;

public record GetProductListQuery() : IRequest<List<ProductDto>>;
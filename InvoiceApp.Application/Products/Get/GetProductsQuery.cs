using InvoiceApp.Application.DTOs;
using InvoiceApp.Domain.Commons.Models;
using MediatR;

namespace InvoiceApp.Application.Products.Get;

public record GetProductsQuery(
  int page,
  int pageSize,
  string? searchTerm
) : IRequest<PagedList<ProductDto>>;
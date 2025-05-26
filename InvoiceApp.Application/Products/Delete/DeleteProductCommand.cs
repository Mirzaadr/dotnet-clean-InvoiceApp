using MediatR;

namespace InvoiceApp.Application.Products.Delete;

public record DeleteProductCommand(
  Guid ProductId
) : IRequest;
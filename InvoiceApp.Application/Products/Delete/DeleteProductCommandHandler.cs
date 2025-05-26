using InvoiceApp.Domain.Products;
using MediatR;

namespace InvoiceApp.Application.Products.Delete;

internal class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
  private readonly IProductRepository _productRepository;

  public DeleteProductCommandHandler(IProductRepository productRepository)
  {
      _productRepository = productRepository;
  }

  public async Task Handle(DeleteProductCommand command, CancellationToken cancellationToken)
  {
      var product = await _productRepository.GetByIdAsync(new ProductId(command.ProductId));
      if (product is null) throw new Exception("Product not found");

      await _productRepository.DeleteAsync(product);
  }
}
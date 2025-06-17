using InvoiceApp.Domain.Products;
using MediatR;

namespace InvoiceApp.Application.Products.Delete;

internal class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
  private readonly IProductRepository _productRepository;
  private readonly IUnitOfWork _unitOfWork;

  public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
  {
      _productRepository = productRepository;
      _unitOfWork = unitOfWork;
  }

  public async Task Handle(DeleteProductCommand command, CancellationToken cancellationToken)
  {
      var product = await _productRepository.GetByIdAsync(ProductId.FromGuid(command.ProductId));
      if (product is null) throw new Exception("Product not found");

      await _productRepository.DeleteAsync(product);
      await _unitOfWork.SaveChangesAsync(cancellationToken);
  }
}
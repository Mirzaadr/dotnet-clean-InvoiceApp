using InvoiceApp.Domain.Products;
using MediatR;

namespace InvoiceApp.Application.Products.Update;

internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

  public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
  {
      _productRepository = productRepository;
      _unitOfWork = unitOfWork;
  }

  public async Task Handle(UpdateProductCommand command, CancellationToken cancellationToken)
  {
    var product = await _productRepository.GetByIdAsync(ProductId.FromGuid(command.Id));
    if (product is null)
    {
      throw new Exception("Not Found");
    }

    product.UpdatePrice(command.Price);
    
    product.UpdateDetails(
      command.Name,
      command.Description
    );
    await _productRepository.UpdateAsync(product);
    await _unitOfWork.SaveChangesAsync(cancellationToken);
  }
}
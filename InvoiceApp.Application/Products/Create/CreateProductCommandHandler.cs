using InvoiceApp.Domain.Products;
using MediatR;

namespace InvoiceApp.Application.Products.Create;

internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

  public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
  {
      _productRepository = productRepository;
      _unitOfWork = unitOfWork;
  }

  public async Task Handle(CreateProductCommand command, CancellationToken cancellationToken)
  {
    // TODO: check by name in database
    await Task.CompletedTask;
    var newProduct = Product.Create(
      command.Name,
      command.Price,
      command.Description
    );

    await _productRepository.AddAsync(newProduct);
    await _unitOfWork.SaveChangesAsync(cancellationToken);
  }
}
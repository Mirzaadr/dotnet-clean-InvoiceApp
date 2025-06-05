using InvoiceApp.Domain.Products;
using MediatR;

namespace InvoiceApp.Application.Products.Update;

internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;

  public UpdateProductCommandHandler(IProductRepository productRepository)
  {
      _productRepository = productRepository;
  }

  public async Task Handle(UpdateProductCommand command, CancellationToken cancellationToken)
  {
    await _productRepository.UpdateAsync(new Product(
      new ProductId(command.Id),
      command.Name,
      command.Price,
      command.Description,
      null,
      null
    ));
    await Task.CompletedTask;
  }
}
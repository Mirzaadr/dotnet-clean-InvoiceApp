using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Domain.Products;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(ProductId id);
    Task<List<Product>> GetAllAsync();
    Task<PagedList<Product>> GetAllAsync(int page, int pageSize, string? searchTerm);
    Task<List<Product>> GetByIdsAsync(IEnumerable<ProductId> ids);

    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}

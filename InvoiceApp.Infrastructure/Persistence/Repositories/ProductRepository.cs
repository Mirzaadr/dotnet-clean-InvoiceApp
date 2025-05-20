using InvoiceApp.Domain.Products;

namespace InvoiceApp.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly InMemoryDbContext _context;

    public ProductRepository(InMemoryDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(Product product)
    {
        var existingProduct = _context.Products.FirstOrDefault(i => i.Id == product.Id);
        if (existingProduct is null)
        {
            _context.Add(product);
        }
        else
        {
            existingProduct = product;
        }
        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Product product)
    {
        var existingProduct = _context.Products.FirstOrDefault(i => i.Id == product.Id);
        if (existingProduct is null)
        {
            throw new Exception("Product not found");
        }
        else
        {
            _context.Products.Remove(product);
        }
        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task<List<Product>> GetAllAsync() => Task.FromResult(_context.Products);

    public Task<Product?> GetByIdAsync(ProductId id)
    {
        var product = _context.Products.FirstOrDefault(i => i.Id == id);
        return Task.FromResult(product);
    }

    public Task<List<Product>> GetByIdsAsync(IEnumerable<ProductId> ids)
    {
        var products = _context.Products.Where(i => ids.Contains(i.Id)).ToList();
        return Task.FromResult(products);
    }

    public Task UpdateAsync(Product product)
    {
        var existingProduct = _context.Products.FirstOrDefault(i => i.Id == product.Id);
        if (existingProduct is null)
        {
            _context.Add(product);
        }
        else
        {
            existingProduct = product;
        }
        _context.SaveChanges();
        return Task.CompletedTask;
    }
}
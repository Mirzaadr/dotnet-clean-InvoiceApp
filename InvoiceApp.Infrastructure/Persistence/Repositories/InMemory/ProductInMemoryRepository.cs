using InvoiceApp.Application.Commons.Interface;
using InvoiceApp.Domain.Commons.Models;
using InvoiceApp.Domain.Products;

namespace InvoiceApp.Infrastructure.Persistence.Repositories.InMemory;

public class ProductInMemoryRepository : IProductRepository
{
    private readonly InMemoryDbContext _context;

    private readonly ICacheService _cache;
    private static readonly string CacheKey = "products_cache";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    public ProductInMemoryRepository(InMemoryDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public Task AddAsync(Product product)
    {
        _context.Products.Add(product);

        // Clear cache after adding new product
        _cache.Remove(CacheKey);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);

        // Clear cache after removing product
        _cache.Remove(CacheKey);
        return Task.CompletedTask;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        var products = await _cache.GetOrCreateAsync(
            CacheKey, 
            () => Task.FromResult(_context.Products), CacheDuration);
        return products;
    }

    public async Task<PagedList<Product>> GetAllAsync(int page, int pageSize, string? searchTerm)
    {
        var productQuery = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            productQuery = productQuery.Where(i =>
                i.Name.ToLower().Contains(searchTerm) ||
                (i.Description != null && i.Description.ToLower().Contains(searchTerm))
            );
        }

        return await PagedList<Product>.CreateAsync(productQuery, page, pageSize);
    }

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
        _cache.Remove(CacheKey);

        return Task.CompletedTask;
    }
}
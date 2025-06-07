using InvoiceApp.Application.Commons.Interface;
using InvoiceApp.Domain.Commons.Models;
using InvoiceApp.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Persistence.Repositories.Db;

public class ProductDbRepository : IProductRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    private readonly ICacheService _cache;
    private static readonly string CacheKey = "products_cache";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    public ProductDbRepository(ICacheService cache, IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _cache = cache;
        _dbContextFactory = dbContextFactory;
    }

    public async Task AddAsync(Product product)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
        }
        // Clear cache after adding new product
        _cache.Remove(CacheKey);
    }

    public async Task DeleteAsync(Product product)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
        }
        // Clear cache after removing product
        _cache.Remove(CacheKey);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            var products = await _cache.GetOrCreateAsync(
                CacheKey, 
                async () => await dbContext.Products.ToListAsync(), CacheDuration);
            return products;
        }
    }

    public async Task<PagedList<Product>> GetAllAsync(int page, int pageSize, string? searchTerm)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            
            var productQuery = dbContext.Products.AsQueryable();

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
    }

    public async Task<Product?> GetByIdAsync(ProductId id)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            var product = dbContext.Products.FirstOrDefault(i => i.Id == id);
            return product;
        }
    }

    public async Task<List<Product>> GetByIdsAsync(IEnumerable<ProductId> ids)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            var products = dbContext.Products.Where(i => ids.Contains(i.Id)).ToList();
            return products;
        }
    }

    public async Task UpdateAsync(Product product)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            var existingProduct = dbContext.Products.FirstOrDefault(i => i.Id == product.Id);
            if (existingProduct is null)
            {
                throw new KeyNotFoundException($"Product with ID {product.Id} not found.");
            }

            if (existingProduct.UnitPrice != product.UnitPrice)
            {
                existingProduct.UpdatePrice(product.UnitPrice);
            }

            existingProduct.UpdateDetails(product.Name, product.Description);
            dbContext.SaveChanges();
            _cache.Remove(CacheKey);
        }
    }
}
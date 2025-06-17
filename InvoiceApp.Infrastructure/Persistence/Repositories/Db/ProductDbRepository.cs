using InvoiceApp.Application.Commons.Interface;
using InvoiceApp.Domain.Commons.Models;
using InvoiceApp.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Persistence.Repositories.Db;

public class ProductDbRepository : IProductRepository
{
    private readonly AppDbContext _context;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    private readonly ICacheService _cache;
    private static readonly string CacheKey = "products_cache";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    public ProductDbRepository(ICacheService cache, AppDbContext context, IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _cache = cache;
        _context = context;
        _dbContextFactory = dbContextFactory;
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        // Clear cache after adding new product
        _cache.Remove(CacheKey);
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
        var product = await _context.Products.FirstOrDefaultAsync(i => i.Id == id);
        return product;
    }

    public async Task<List<Product>> GetByIdsAsync(IEnumerable<ProductId> ids)
    {
        using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
        {
            var products = await dbContext.Products.Where(i => ids.Contains(i.Id)).ToListAsync();
            return products;
        }
    }

    public Task UpdateAsync(Product product)
    {
        _context.Set<Product>().Update(product);
        _cache.Remove(CacheKey);
        return Task.CompletedTask;
    }
}
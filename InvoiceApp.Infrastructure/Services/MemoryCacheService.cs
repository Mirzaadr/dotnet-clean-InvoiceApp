using Microsoft.Extensions.Caching.Memory;
using InvoiceApp.Application.Commons.Interface;

namespace InvoiceApp.Infrastructure.Services;

public class MemoryCacheService : ICacheService
{
  private readonly IMemoryCache _cache;

  public MemoryCacheService(IMemoryCache cache)
  {
    _cache = cache;
  }

  public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiration)
  {
    if (_cache.TryGetValue<T>(key, out var value))
      return value!;

    var result = await factory();

    _cache.Set(key, result, new MemoryCacheEntryOptions
    {
      AbsoluteExpirationRelativeToNow = expiration
    });

    return result;
  }

  public void Remove(string key)
  {
    _cache.Remove(key);
  }
}

namespace InvoiceApp.Application.Commons.Interface;

public interface ICacheService
{
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan expiration);
    void Remove(string key);
}

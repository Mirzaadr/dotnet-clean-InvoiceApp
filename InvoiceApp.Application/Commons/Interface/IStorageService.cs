namespace InvoiceApp.Application.Commons.Interface;

public interface IStorageService
{
  Task<byte[]?> GetFileAsync(string fileName, CancellationToken cancellationToken = default);
  Task<string?> SaveFileAsync(string fileName, byte[] fileContent, CancellationToken cancellationToken = default);
  // Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken = default);
  // Task<bool> FileExistsAsync(string fileName, CancellationToken cancellationToken = default);
}
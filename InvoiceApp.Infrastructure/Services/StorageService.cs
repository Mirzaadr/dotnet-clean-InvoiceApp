using InvoiceApp.Application.Commons.Interface;
using Microsoft.Extensions.Configuration;

namespace InvoiceApp.Infrastructure.Services;

internal sealed class StorageService : IStorageService
{
    private readonly IConfiguration _config;

    public StorageService(IConfiguration config)
    {
        _config = config;
    }

    public Task<byte[]?> GetFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        // Retrieve file from local folder
        Console.WriteLine($"Retrieving file: {fileName}");
        var directory = Path.Combine("wwwroot", "invoices");
        var filePath = Path.Combine(directory, fileName);

        if (!File.Exists(filePath))
        {
            return Task.FromResult<byte[]?>(null);
        }

        try
        {
            var fileBytes = File.ReadAllBytes(filePath);
            return Task.FromResult<byte[]?>(fileBytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
            return Task.FromResult<byte[]?>(null);
        }
    }

    public Task<string?> SaveFileAsync(string fileName, byte[] fileContent, CancellationToken cancellationToken = default)
    {
        // Simulate file saving
        Console.WriteLine($"Saving file: {fileName}");
        try
        {
            // Ensure the directory exists
            var directory = Path.Combine("wwwroot", "invoices");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var filePath = Path.Combine(directory, $"{fileName}.pdf");
            File.WriteAllBytes(filePath, fileContent);

            var baseUrl = _config["App:BaseUrl"] ?? "http://localhost:3000";
            return Task.FromResult<string?>($"{baseUrl}/Invoice/Download/{fileName}"); // Assume save is successful
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating directory: {ex.Message}");
            return Task.FromResult<string?>(null);
        }
    }
}
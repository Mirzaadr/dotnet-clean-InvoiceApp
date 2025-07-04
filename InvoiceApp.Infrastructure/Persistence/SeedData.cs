using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Invoices;
using Bogus;
using InvoiceApp.Domain.Products;
using InvoiceApp.Domain.Users;
using InvoiceApp.Application.Commons.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.Infrastructure.Persistence;

public static class SeedData
{
    // Helper function to generate random invoice status (either Pending or Paid)
    private static InvoiceStatus GetRandomStatus()
    {
        Faker f = new Faker();
        var status = f.PickRandom(
            new[]
            {
                "Draft",
                "Sent",
                "Paid"
            }
        );
        return InvoiceStatus.From(status);
    }

    public static void Seed(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<InMemoryDbContext>();
        var hasher = serviceProvider.GetRequiredService<IPasswordHasher>();

        int clientCount = 50;
        int productCount = 300;
        int invoiceCount = 100;

        var clientFaker = new Faker<Client>()
            .CustomInstantiator(f => Client.Create(
                f.Company.CompanyName(),
                f.Address.FullAddress(),
                f.Internet.Email(),
                f.Phone.PhoneNumber()
            ));

        var productFaker = new Faker<Product>()
            .CustomInstantiator(f => Product.Create(
                f.Commerce.ProductName(),
                f.Random.Double(10, 500),
                f.Commerce.ProductDescription()
            ));

        var clients = clientFaker.Generate(clientCount);
        var products = productFaker.Generate(productCount);

        foreach (var client in clients)
            context.Clients.Add(client);

        foreach (var product in products)
            context.Products.Add(product);

        // Seed Invoices using the created clients/products
        var invoiceNumberSeed = 1000;
        var random = new Random();

        for (int i = 0; i < invoiceCount; i++)
        {
            List<InvoiceItem> items = new List<InvoiceItem>();
            var itemCount = random.Next(1, 4);
            for (int j = 0; j < itemCount; j++)
            {
                var product = products[random.Next(products.Count)];
                var quantity = random.Next(1, 5);
                items.Add(InvoiceItem.Create(
                    product.Id,
                    product.Name,
                    product.UnitPrice,
                    quantity
                ));
            }

            var client = clients[random.Next(clients.Count)];
            var invoice = Invoice.Create(
                client.Id,
                client.Name,
                $"INV-{invoiceNumberSeed + i}-2025",
                DateTime.Today.AddDays(-random.Next(30)),
                DateTime.Now.AddDays(random.Next(30)),
                GetRandomStatus(),
                items
            );

            context.Invoices.Add(invoice);
        }

        // IPasswordHasher hasher = new PasswordHasher();

        context.Users.Add(User.Create(
            "admin",
            hasher.Hash("password123")));
    }
}

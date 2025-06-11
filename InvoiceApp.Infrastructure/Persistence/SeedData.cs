using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Invoices;
using Bogus;
using InvoiceApp.Domain.Products;

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

    public static void Seed(InMemoryDbContext context)
    {
        int clientCount = 50;
        int productCount = 300;
        int invoiceCount = 100;

        var clientFaker = new Faker<Client>()
            .CustomInstantiator(f => new Client(
                ClientId.New(),
                f.Company.CompanyName(),
                f.Address.FullAddress(),
                f.Internet.Email(),
                f.Phone.PhoneNumber(),
                DateTime.Now,
                DateTime.Now
            ));

        var productFaker = new Faker<Product>()
            .CustomInstantiator(f => new Product(
                ProductId.New(),
                f.Commerce.ProductName(),
                f.Random.Double(10, 500),
                f.Commerce.ProductDescription(),
                DateTime.Now,
                DateTime.Now
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
                items.Add(new InvoiceItem(
                    InvoiceItemId.New(),
                    product.Id,
                    product.Name,
                    product.UnitPrice,
                    quantity,
                    DateTime.Now,
                    DateTime.Now
                ));
            }

            var client = clients[random.Next(clients.Count)];
            var invoice = new Invoice(
                InvoiceId.New(),
                client.Id,
                client.Name,
                $"INV-{invoiceNumberSeed + i}-2025",
                DateTime.Today.AddDays(-random.Next(30)),
                DateTime.Now.AddDays(random.Next(30)),
                GetRandomStatus(),
                items,
                DateTime.Now,
                DateTime.Now
            );

            context.Invoices.Add(invoice);
        }
    }
}

using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Invoices;

namespace InvoiceApp.Infrastructure.Persistence;
public static class SeedData
{
    public static void Initialize(InMemoryDbContext context)
    {
        // Add Clients
        context.Clients.AddRange(
          Client.Create("Client A", null, "clientA@example.com", null),
          Client.Create("Client B", null, "clientB@example.com", null),
          Client.Create("Client C", null, "clientC@example.com", null)
        );

        // Add Invoices
        var invoices = new List<Invoice>();

        for (int i = 1; i <= 10; i++)  // Create 10 invoices (you can increase the number if needed)
        {
            var client = GetRandomClient(context.Clients);

            // Add random items to the invoice
            var invoiceItems = new List<LineItem>();
            var numItems = RandomNumber(1, 5); // Each invoice can have 1 to 5 items
            for (int j = 0; j < numItems; j++)
            {
                invoiceItems.Add(LineItem.Create(
                    // j + 1,  // Item ID (can be sequential, or random)
                    // invoice.InvoiceId,
                    "Item " + i,
                    GetRandomDescription(),
                    RandomNumber(1, 5), // Quantity between 1 and 5
                    GetRandomAmount()   // Random amount for the item
                ));
            }


            var invoice = Invoice.Create(
                // i, 
                client.Id,  // Cycle through the 3 clients
                DateTime.Now.AddDays(-RandomNumber(0, 30)),
                DateTime.Now.AddDays(RandomNumber(30, 60)),
                GetRandomStatus(),
                // GetRandomAmount(),
                invoiceItems
            );

            // Add the invoice and its items
            invoices.Add(invoice);
        }

        context.Invoices.AddRange(invoices);
        context.SaveChanges();
    }

    // Helper function to generate random amounts (within a range)
    private static double GetRandomAmount()
    {
        return RandomNumber(50, 1000);  // Random amount between $50 and $1000
    }

    // Helper function to generate random invoice status (either Pending or Paid)
    private static InvoiceStatus GetRandomStatus()
    {
        return (RandomNumber(0, 1) == 0) 
          ? new InvoiceStatus(InvoiceStatusType.Pending) 
          : new InvoiceStatus(InvoiceStatusType.Paid);
    }

    // Helper function to generate random numbers within a range
    private static int RandomNumber(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max);
    }

    // Helper function to generate random description
    private static string GetRandomDescription()
    {
        var descriptions = new List<string>
        {
            "Web Development Services",
            "Graphic Design Services",
            "Consulting",
            "Project Management",
            "Hosting Services",
            "Software License",
            "Mobile App Development",
            "SEO Optimization"
        };
        Random random = new Random();
        return descriptions[random.Next(descriptions.Count)];
    }

    private static Client GetRandomClient(List<Client> clients)
    {
        Random random = new Random();
        return clients[random.Next(clients.Count)];
    }
}

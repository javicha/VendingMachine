using Microsoft.Extensions.Logging;
using Vending.Domain.Entities;

namespace Vending.Infrastructure.Persistence
{
    /// <summary>
    /// Class to insert initial data at application startup
    /// </summary>
    public class VendingContextSeed
    {
        public static async Task SeedAsync(VendingContext vendingContext, ILogger<VendingContextSeed>? logger)
        {
            if (!vendingContext.VendingMachines.Any() && !vendingContext.Products.Any())
            {
                vendingContext.VendingMachines.Add(GetVendingMachine());
                await vendingContext.SaveChangesAsync();
                logger?.LogInformation("Seed database associated with context {DbContextName}", typeof(VendingContext).Name);
            }
        }

        private static VendingMachine GetVendingMachine()
        {
            string userCreated = "javier.val";
            var vendingMachine = new VendingMachine("1234", "Delicious Drinks", userCreated);
            foreach(var product in GetProducts())
            {
                vendingMachine.AddNewProduct(userCreated, product.Name, product.Price, product.Portions);
            }

            return vendingMachine;
        }

        private static IEnumerable<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product("Tea", 1.30M, 10),
                new Product("Espresso", 1.80M, 20),
                new Product("Juice", 1.80M, 20),
                new Product("Chicken soup", 1.80M, 15)
            };
        }
    }
}

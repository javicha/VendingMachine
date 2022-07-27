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
            foreach(var coin in GetCoins())
            {
                vendingMachine.AddNewCoin(userCreated, coin);
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

        private static IEnumerable<Coin>  GetCoins()
        {
            //10 cent, 100 coins
            //20 cent, 100 coins
            //50 cent, 100 coins
            //1 euro, 100 coins
            List<Coin> coins = new List<Coin>();
            for (int i = 0; i < 100; i++)
            {
                coins.Add(new Coin(0.10M));
                coins.Add(new Coin(0.20M));
                coins.Add(new Coin(0.50M));
                coins.Add(new Coin(1M));
            }
            return coins;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vending.Domain.Entities;
using Vending.Infrastructure.Persistence;
using Vending.Infrastructure.Repositories;

namespace Vending.Test.Infrastructure
{
    /// <summary>
    /// Test class to ensure the correct operations in ProductRepository
    /// </summary>
    public class ProductRepositoryTest
    {
        private readonly VendingContext dbContext;
        private readonly DbContextOptions<VendingContext> dbContextOptions;
        private readonly ProductRepository productRepository;

        public ProductRepositoryTest()
        {
            //In Memory Database Provider for Testing
            // Build DbContextOptions
            dbContextOptions = new DbContextOptionsBuilder<VendingContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) //use Guid so every test use a different database
                .Options;

            dbContext = new VendingContext(dbContextOptions, null);
            productRepository = new ProductRepository(dbContext);
        }


        [Fact]
        public async Task AddProduct_Ok()
        {
            //Variables
            var productToSave = GetProduct();

            // Execute
            var newProduct = await productRepository.AddAsync(productToSave, "userTest");

            //Assert
            Assert.NotNull(newProduct);
            Assert.Equal(productToSave.Name, newProduct.Name);
            Assert.Equal(productToSave.Price, newProduct.Price);
            Assert.Equal(productToSave.Portions, newProduct.Portions);
            Assert.Equal(productToSave.MinStock, newProduct.MinStock);
            Assert.True(await productRepository.ExistByName(newProduct.Name));
        }


        private Product GetProduct()
        {
            return new Product("Product Test", 1M, 10, 1);
        }
    }
}

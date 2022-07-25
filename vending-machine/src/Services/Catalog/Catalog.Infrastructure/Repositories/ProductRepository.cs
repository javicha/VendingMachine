using Microsoft.EntityFrameworkCore;
using Vending.Application.Contracts.Persistence;
using Vending.Domain.Entities;
using Vending.Infrastructure.Persistence;

namespace Vending.Infrastructure.Repositories
{
    /// <summary>
    /// Class containing the implementation of the specific methods for ProductRepository
    /// </summary>
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(VendingContext vendingContext) : base(vendingContext) { }

        /// <summary>
        /// Check if a product exists from its name
        /// </summary>
        /// <param name="productName">Exact name of the product</param>
        /// <returns>True if the product exists. False otherwise</returns>
        public Task<bool> ExistByName(string productName)
        {
            string name = productName.Trim().ToLower();
            return _vendingContext.Products.AnyAsync(x => x.Name.ToLower() == name && x.DateDeleted == null);
        }

        /// <summary>
        /// Get the list of all available products of a vending machine
        /// </summary>
        /// <param name="vendingMachineId">Vending machine identifier</param>
        /// <returns>The product catalog of the vending machine</returns>
        public Task<List<Product>> GetAllProductsByVendingMachine(int vendingMachineId)
        {
            return _vendingContext.Products.Where(p => p.DateDeleted == null && p.Portions > 0).ToListAsync();
        }

        /// <summary>
        /// Get a product based on its exact name
        /// </summary>
        /// <param name="productName">Exact name of the product</param>
        /// <returns>The product, if exists. Null otherwise</returns>
        public Task<Product?> GetByName(string productName)
        {
            string name = productName.Trim().ToLower();
            return _vendingContext.Products.Where(x => x.Name.ToLower() == name && x.DateDeleted == null).FirstOrDefaultAsync();
        }
    }
}
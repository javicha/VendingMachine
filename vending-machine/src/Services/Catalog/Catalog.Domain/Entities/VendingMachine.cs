using Vending.Domain.Common;
using Vending.Domain.ExtensionMethods;

namespace Vending.Domain.Entities
{
    /// <summary>
    /// Entity that models a vending machine
    /// </summary>
    public class VendingMachine : EntityBase
    {
        /// <summary>
        /// Vending machine serial number
        /// </summary>
        public string SerialNumber { get; private set; }

        /// <summary>
        /// Vending machine serial name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Deposited amount to perform a purchase
        /// </summary>
        public CoinBasket CoinBasket { get; private set; }

        /// <summary>
        /// Set of products available in the vending machine. This is the catalog of the vending machine
        /// </summary>
        private readonly HashSet<Product> _products;
        public IReadOnlyCollection<Product> Products => _products.ToList().AsReadOnly();


        public VendingMachine(string serialNumber, string name, string userName)
        {
            _products = new HashSet<Product>();
            CoinBasket = new CoinBasket();
            SerialNumber = serialNumber ?? throw new ArgumentNullException(nameof(serialNumber));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            this.SetInsertAuditParams(userName);
        }


        #region Methods

        /// <summary>
        /// Get the catalog with all the products of the machine
        /// </summary>
        /// <returns>The list of available products</returns>
        public IEnumerable<Product> GetProductCatalog()
        {
            return _products.Where(g => g.DateDeleted == null);
        }

        /// <summary>
        /// Add a new product to the catalog
        /// </summary>
        /// <param name="name">Product name</param>
        /// <param name="price">Sale price</param>
        /// <param name="portions">Number of portions available</param>
        /// <param name="minStock">Critical Stock. When the product reaches the number of units indicated in this property, a notification could be launched in order to replenish the product.</param>
        /// <param name="userName">User performing the action</param>
        public void AddNewProduct(string userName, string name, decimal price, int portions, int minStock = 0)
        {
            _products.Add(new Product(name, price, portions, minStock).SetInsertAuditParams(userName));
        }

        #endregion
    }
}

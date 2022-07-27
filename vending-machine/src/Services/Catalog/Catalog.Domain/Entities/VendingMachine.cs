using Domain.VO;
using Vending.Domain.Common;
using Vending.Domain.Events;
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
        /// Set of products available in the vending machine. This is the catalog of the vending machine
        /// </summary>
        private readonly HashSet<Product> _products;
        public IReadOnlyCollection<Product> Products => _products.ToList().AsReadOnly();

        /// <summary>
        /// Set of coins inserted in the vending machine. Represents the deposited amount to perform a purchase
        /// </summary>
        private readonly HashSet<Coin> _coins;
        public IReadOnlyCollection<Coin> Coins => _coins.ToList().AsReadOnly();


        public VendingMachine(string serialNumber, string name, string userCreated)
        {
            _products = new HashSet<Product>();
            _coins = new HashSet<Coin>();
            SerialNumber = serialNumber ?? throw new ArgumentNullException(nameof(serialNumber));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            this.SetInsertAuditParams(userCreated);
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

        /// <summary>
        /// Add a coin to the vending machine wallet
        /// </summary>
        /// <param name="coin"></param>
        /// <param name="userName">User performing the action</param>
        public void AddNewCoin(string userName, Coin coin)
        {
            _coins.Add(coin.SetInsertAuditParams(userName));
        }

        /// <summary>
        /// Insert a coin into the vending machine. Used by the client in order to buy a product
        /// </summary>
        /// <param name="coin">A valid coin</param>
        /// <returns>The total amount entered so far</returns>
        public decimal InsertCoin(decimal amount)
        {
            PublishDomainEvent(new IncreaseWalletEvent(amount, SerialNumber));
            return _coins.Where(c => c.External).Sum(c => c.Amount) + amount;
        }

        /// <summary>
        /// Take back the coins inserted in the vending machine. Used by the client in order to cancel a purchase
        /// </summary>
        /// <returns>All the coins inserted</returns>
        public List<Coin> ReturnCoins()
        {
            List<Coin> coinsToReturn = _coins.Where(c => c.External).ToList();
            PublishDomainEvent(new GetBackCoinsEvent(SerialNumber));
            return coinsToReturn;
        }

        /// <summary>
        /// Update the wallet of the vending machine by adding a coin with the indicated amount
        /// </summary>
        /// <param name="amount"></param>
        public void IncreaseWallet(decimal amount)
        {
            var coin = new Coin(amount, true);
            _coins.Add(coin);
        }

        /// <summary>
        /// Remove the coins inserted by the client
        /// </summary>
        public void RemoveExternalCoins()
        {
            _coins.Where(c => c.External).ToList().ForEach(c =>
            {
                c.SetDeleteAuditParams("userTest");
            });
        }

        #endregion
    }
}

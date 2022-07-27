using Vending.Domain.Common;
using Vending.Domain.Events;

namespace Vending.Domain.Entities
{
    /// <summary>
    /// Entity that models a vending machine product
    /// </summary>
    public class Product : EntityBase
    {
        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Sale price
        /// </summary>
        public decimal Price { get; private set; }

        /// <summary>
        /// Number of portions available
        /// </summary>
        public int Portions { get; private set; }

        /// <summary>
        /// Critical Stock. When the product reaches the number of units indicated in this property, a notification could be launched in order to replenish the product.
        /// </summary>
        public int MinStock { get; private set; }

        /// <summary>
        /// Vending machine to which the products belong
        /// </summary>
        public VendingMachine VendingMachine { get; private set; }
        public int VendingMachineId { get; set; }


        public Product(string name, decimal price, int portions, int minStock)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Price = price;
            Portions = portions;
            MinStock = minStock;
        }

        public Product(string name, decimal price, int portions)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Price = price;
            Portions = portions;
            MinStock = 0;
        }



        #region Methods

        /// <summary>
        /// Decrease the stock of the product and create a domain event to calculate and return the price difference
        /// </summary>
        /// <param name="availableBalance">Balance available for purchase</param>
        public void SellProduct(decimal availableBalance)
        {
            DecreasePortion();
            AddDomainEvent(new ReturnDifferenceEvent() { AvailableBalance = availableBalance, ProductPrice = Price});
        }

        /// <summary>
        /// Decrements the number of portions of the product by one unit. The minimum number of units will be zero.
        /// </summary>
        private void DecreasePortion()
        {
            if (Portions > 0) 
            { 
                Portions -= 1;

                //Check minimal stock
                if(Portions == MinStock) 
                {
                    //We could create an event to notify that it is necessary to replenish the stock
                }
            }
        }

        #endregion
    }
}

using Domain.Exceptions;
using Vending.Domain.Common;

namespace Vending.Domain.Entities
{
    /// <summary>
    /// Entity used to represent the coins introduced by the customer in the vending machine
    /// </summary>
    public class Coin : EntityBase
    {
        /// <summary>
        /// Monetary quantity
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// For simplicity: True if the coin has been inserted by the customer, False otherwise
        /// </summary>
        public bool External { get; private set; }

        /// <summary>
        /// Vending machine to which the coins belong
        /// </summary>
        public VendingMachine VendingMachine { get; private set; }
        public int VendingMachineId { get; set; }

        public Coin(decimal amount, bool external = false)
        {
            //Ensuring integrity of data
            if (!ValidCoinAmounts.Contains(amount))
            {
                throw new WrongCoinAmountException($"The coin amount {amount} is not supported");
            }

            Amount = amount;
            External = external;
        }

        //For simplicity, we store the currency and allowed coin values in private variables
        private readonly List<decimal> ValidCoinAmounts = new List<decimal>() { 0.10m, 0.20m, 0.50m, 1m, 2m };
    }
}

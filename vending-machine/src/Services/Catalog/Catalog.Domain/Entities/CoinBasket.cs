using Domain.VO;
using System.ComponentModel.DataAnnotations.Schema;
using Vending.Domain.Common;

namespace Vending.Domain.Entities
{
    /// <summary>
    /// Entity used to represent the coins introduced by the customer in the vending machine
    /// </summary>
    public class CoinBasket : EntityBase
    {
        /// <summary>
        /// Set of coins inserted in the vending machine. Represents the deposited amount to perform a purchase
        /// </summary>
        private readonly HashSet<Coin> _coins;
        [NotMapped]
        public IReadOnlyCollection<Coin> Coins => _coins.ToList().AsReadOnly();


        public CoinBasket()
        {
            _coins = new HashSet<Coin>();
        }
    }
}

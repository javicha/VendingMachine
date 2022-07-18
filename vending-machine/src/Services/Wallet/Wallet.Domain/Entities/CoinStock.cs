using Wallet.Domain.Common;
using Wallet.Domain.VO;

namespace Wallet.Domain.Entities
{
    /// <summary>
    /// Entity that models the stock of coins
    /// </summary>
    public class CoinStock : EntityBase
    {
        /// <summary>
        /// Coin with its value and currency
        /// </summary>
        public Coin Coin { get; private set; }

        /// <summary>
        /// Number of available coins of a certain value
        /// </summary>
        public int NumUnits { get; private set; }

        /// <summary>
        /// For simplicity, we use this boolean to distinguish coins entered by an external user in order to purchase products.
        /// True: coin inserted by an external user. False: coin inserted by maintenance personnel.
        /// </summary>
        public bool External { get; private set; }

        public CoinStock(Coin coin, int numUnits, bool external)
        {
            Coin = coin;
            NumUnits = numUnits;
            External = external;
        }


        #region PublicMethods

        #endregion
    }
}

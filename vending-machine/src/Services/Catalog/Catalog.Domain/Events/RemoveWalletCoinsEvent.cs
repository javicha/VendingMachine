using Domain.DTO;
using Domain.Events;

namespace Vending.Domain.Events
{
    /// <summary>
    /// Domain event
    /// </summary>
    public class RemoveWalletCoinsEvent : IDomainEvent
    {
        /// <summary>
        /// Coins to remove in order to update the wallet
        /// </summary>
        public List<CoinDTO> Coins { get; set; }

        /// <summary>
        /// Vending machine serial number
        /// </summary>
        public string SerialNumber { get; set; }

        public RemoveWalletCoinsEvent(List<CoinDTO> coins, string serialNumber)
        {
            Coins = coins ?? throw new ArgumentNullException(nameof(coins));
            SerialNumber = serialNumber ?? throw new ArgumentNullException(nameof(serialNumber));
        }
    }
}

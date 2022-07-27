using Domain.Events;

namespace Vending.Domain.Events
{
    public class IncreaseWalletEvent : IDomainEvent
    {
        /// <summary>
        /// Amount by which the wallet is increased
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Vending machine serial number
        /// </summary>
        public string SerialNumber { get; set; }

        public IncreaseWalletEvent(decimal amount, string serialNumber)
        {
            Amount = amount;
            SerialNumber = serialNumber ?? throw new ArgumentNullException(nameof(serialNumber));
        }
    }
}

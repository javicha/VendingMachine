using Domain.Events;

namespace Vending.Domain.Events
{
    /// <summary>
    /// Domain event
    /// </summary>
    public class GetBackCoinsEvent : IDomainEvent
    {
        /// <summary>
        /// Vending machine serial number
        /// </summary>
        public string SerialNumber { get; set; }

        public GetBackCoinsEvent(string serialNumber)
        {
            SerialNumber = serialNumber ?? throw new ArgumentNullException(nameof(serialNumber));
        }
    }
}

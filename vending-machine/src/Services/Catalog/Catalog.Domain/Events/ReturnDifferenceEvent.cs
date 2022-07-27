using Domain.Events;

namespace Vending.Domain.Events
{
    public class ReturnDifferenceEvent : IDomainEvent
    {
        public decimal ProductPrice { get; set; }
        public decimal AvailableBalance { get; set; }
    }
}

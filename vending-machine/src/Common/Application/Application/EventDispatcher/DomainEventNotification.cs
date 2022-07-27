using Domain.Events;
using MediatR;

namespace Application.EventDispatcher
{
    /// <summary>
    /// Our Domain Event not being a valid MediatR INotification. We need to create a generic INotification to wrap our Domain Event.
    /// </summary>
    public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : IDomainEvent
    {
        public TDomainEvent DomainEvent { get; }

        public DomainEventNotification(TDomainEvent domainEvent)
        {
            DomainEvent = domainEvent;
        }
    }
}

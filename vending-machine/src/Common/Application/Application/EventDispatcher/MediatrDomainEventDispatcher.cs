using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.EventDispatcher
{
    /// <summary>
    /// Dispatcher that wraps Domain Events in MediatR notificatoins and publishes them
    /// </summary>
    public class MediatrDomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MediatrDomainEventDispatcher> _log;
        public MediatrDomainEventDispatcher(IMediator mediator, ILogger<MediatrDomainEventDispatcher> log)
        {
            _mediator = mediator;
            _log = log;
        }

        public async Task Dispatch(IDomainEvent @event)
        {

            var domainEventNotification = CreateDomainEventNotification(@event);
            _log.LogDebug("Dispatching Domain Event as MediatR notification.  EventType: {eventType}", @event.GetType());
            await _mediator.Publish(domainEventNotification);
        }

        private INotification CreateDomainEventNotification(IDomainEvent domainEvent)
        {
            var genericDispatcherType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            return (INotification)Activator.CreateInstance(genericDispatcherType, domainEvent);
        }
    }
}

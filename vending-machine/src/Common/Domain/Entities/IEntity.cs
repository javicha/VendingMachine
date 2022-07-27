using Domain.Events;
using System.Collections.Concurrent;

namespace Domain.Entities
{
    public interface IEntity
    {
        IProducerConsumerCollection<IDomainEvent> DomainEvents { get; }
    }
}

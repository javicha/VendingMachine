using Domain.Events;

namespace Wallet.Domain.Common
{
    /// <summary>
    /// Class with the fields common to all domain entities (for example unique identifier or audit data)
    /// </summary>
    public abstract class EntityBase
    {
        public int Id { get; protected set; } //Protected set in order to use in derived classes
        public DateTime DateCreated { get; set; }

        private readonly List<IDomainEvent> changes;

        protected EntityBase()
        {
            changes = new List<IDomainEvent>();
        }


        protected void AddDomainEvent(IDomainEvent @event)
        {
            this.changes.Add(@event);
        }
    }
}

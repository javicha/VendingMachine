using Domain.Events;

namespace Vending.Domain.Common
{
    /// <summary>
    /// Class with the fields common to all domain entities (for example unique identifier or audit data)
    /// </summary>
    public abstract class EntityBase
    {
        public int Id { get; protected set; } //Protected set in order to use in derived classes
        public string? UserCreated { get; set; }
        public DateTime DateCreated { get; set; }
        public string? UserModified { get; set; }
        public DateTime DateModified { get; set; }
        public string? UserDeleted { get; set; }
        public DateTime? DateDeleted { get; set; }

        private readonly List<IDomainEvent> Changes;

        protected EntityBase()
        {
            Changes = new List<IDomainEvent>();
        }

        public List<IDomainEvent> GetUncommittedEvents()
        {
            return Changes;
        }

        public void MarkEventsAsCommitted()
        {
            Changes.Clear();
        }

        protected void AddDomainEvent(IDomainEvent @event)
        {
            Changes.Add(@event);
        }

        //We could manage concurrency with the RowVersion property. For simplicity it is omitted from this technical test
        //[Timestamp]
        //public byte[] RowVersion { get; private set; }

    }
}

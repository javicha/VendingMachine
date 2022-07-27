using Domain.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Vending.Domain.Common;
using Vending.Domain.Entities;

namespace Vending.Infrastructure.Persistence
{
    /// <summary>
    /// We use EntityFrameworkCore InMemory as ORM system. Our data context must inherit from DbContext, which in turn belongs to EntityframeworkCore
    /// </summary>
    public class VendingContext : DbContext
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public VendingContext(DbContextOptions<VendingContext> options, IPublishEndpoint publishEndpoint) : base(options)
        {
            _publishEndpoint = publishEndpoint;
        }

        public DbSet<VendingMachine> VendingMachines { get; set; }
        public DbSet<Product> Products { get; set; }

        #region Override

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Dispatch Domain Events collection.
            // For simplicity BEFORE committing data (EF SaveChanges) into the DB. This makes
            // a single transaction including side effects from the domain event
            // handlers that are using the same DbContext with Scope lifetime
            List<IDomainEvent> preEvents;
            do
            {
                var entities =  this.ChangeTracker.Entries<EntityBase>().ToList();
                preEvents = entities.SelectMany(x => x.Entity.GetUncommittedEvents()).ToList();
                entities.ForEach(entity => entity.Entity.MarkEventsAsCommitted());
                var tasksPre = preEvents.Select(async r => await _publishEndpoint.Publish(r)).ToList();
                await Task.WhenAll(tasksPre);
            } while (preEvents.Count != 0);

            return await base.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}

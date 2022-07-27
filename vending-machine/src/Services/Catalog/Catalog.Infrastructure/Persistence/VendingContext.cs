using Domain.Entities;
using Domain.Events;
using Microsoft.EntityFrameworkCore;
using Vending.Domain.Entities;

namespace Vending.Infrastructure.Persistence
{
    /// <summary>
    /// We use EntityFrameworkCore InMemory as ORM system. Our data context must inherit from DbContext, which in turn belongs to EntityframeworkCore
    /// </summary>
    public class VendingContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public VendingContext(DbContextOptions<VendingContext> options, IDomainEventDispatcher dispatcher) : base(options)
        {
            _dispatcher = dispatcher;
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
            await DispatchDomainEvents();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private async Task DispatchDomainEvents()
        {
            var domainEventEntities = ChangeTracker.Entries<IEntity>()
                .Select(po => po.Entity)
                .Where(po => po.DomainEvents.Any())
                .ToArray();

            foreach (var entity in domainEventEntities)
            {
                IDomainEvent dev;
                while (entity.DomainEvents.TryTake(out dev))
                    await _dispatcher.Dispatch(dev);
            }
        }

        #endregion
    }
}

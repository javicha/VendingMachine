using Microsoft.EntityFrameworkCore;
using Vending.Domain.Entities;

namespace Vending.Infrastructure.Persistence
{
    /// <summary>
    /// We use EntityFrameworkCore InMemory as ORM system. Our data context must inherit from DbContext, which in turn belongs to EntityframeworkCore
    /// </summary>
    public class VendingContext : DbContext
    {
        public VendingContext(DbContextOptions<VendingContext> options) : base(options)
        {
        }

        public DbSet<VendingMachine> VendingMachines { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}

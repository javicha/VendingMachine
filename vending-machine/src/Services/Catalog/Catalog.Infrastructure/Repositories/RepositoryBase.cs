using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vending.Application.Contracts.Persistence;
using Vending.Domain.Common;
using Vending.Domain.ExtensionMethods;
using Vending.Infrastructure.Persistence;

namespace Vending.Infrastructure.Repositories
{
    /// <summary>
    /// Class that contains the implementation of the methods common to all repositories
    /// </summary>
    public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
    {
        protected readonly VendingContext _vendingContext; //Reachable to subclases

        public RepositoryBase(VendingContext vendingContext)
        {
            _vendingContext = vendingContext ?? throw new ArgumentNullException(nameof(vendingContext));
        }

        /// <summary>
        /// Save an entity to database
        /// </summary>
        /// <param name="entity">Given entity</param>
        /// <returns></returns>
        public async Task<T> AddAsync(T entity, string userName)
        {
            entity.SetInsertAuditParams(userName);
            _vendingContext.Set<T>().Add(entity);
            await _vendingContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Removes a given entity from database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task DeleteAsync(T entity)
        {
            _vendingContext.Set<T>().Remove(entity);
            await _vendingContext.SaveChangesAsync();
        }

        /// <summary>
        /// Sof delete for entity. It does not delete the entity from the database. It only sets a deletion date
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        /// <param name="userName">User performing the deletion</param>
        /// <returns></returns>
        public async Task SoftDeleteAsync(T entity, string userName)
        {
            entity.SetDeleteAuditParams(userName);
            _vendingContext.Entry(entity).State = EntityState.Modified;
            await _vendingContext.SaveChangesAsync();
        }

        /// <summary>
        /// Get a list of all entities of a given type
        /// </summary>
        /// <returns>List of all entities</returns>
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _vendingContext.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Get a list of entities matching a given predicate
        /// </summary>
        /// <param name="predicate">List of entities</param>
        /// <returns></returns>
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _vendingContext.Set<T>().Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Unique identifier</param>
        /// <returns>The entity with the given identifier</returns>
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _vendingContext.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Update a given entity in database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task UpdateAsync(T entity, string userName)
        {
            entity.SetUpdateAuditParams(userName);
            _vendingContext.Entry(entity).State = EntityState.Modified;
            await _vendingContext.SaveChangesAsync();
        }
    }
}

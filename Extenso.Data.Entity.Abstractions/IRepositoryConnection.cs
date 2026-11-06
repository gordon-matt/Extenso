using System.Linq.Expressions;

namespace Extenso.Data.Entity;

/// <summary>
/// Defines the contract for the repository connection.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IRepositoryConnection<TEntity> : IDisposable
{
    /// <summary>
    /// Queries the value.
    /// </summary>
    /// <param name="includePaths">The include paths.</param>
    /// <returns>The t entity i queryable.</returns>
    IQueryable<TEntity> Query(params Expression<Func<TEntity, dynamic>>[] includePaths);

    /// <summary>
    /// Queries the value.
    /// </summary>
    /// <param name="predicate">The predicate to apply.</param>
    /// <param name="includePaths">The include paths.</param>
    /// <returns>The t entity i queryable.</returns>
    IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] includePaths);
}
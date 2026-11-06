using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity;

/// <summary>
/// Defines the contract for the entity framework repository connection.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IEntityFrameworkRepositoryConnection<TEntity> : IRepositoryConnection<TEntity>
{
    /// <summary>
    /// Gets the context.
    /// </summary>
    DbContext Context { get; }
}
using System.Linq.Expressions;
using Extenso.Collections;
using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity;

/// <summary>
/// Represents the mapped entity framework repository connection.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TModel">The type of the model.</typeparam>
public class MappedEntityFrameworkRepositoryConnection<TEntity, TModel> : IEntityFrameworkRepositoryConnection<TModel>
    where TEntity : class
{
    #region Private Members

    private readonly bool isContextOwner;
    private bool disposed;

    private readonly IEntityModelMapper<TEntity, TModel> entityModelMapper;

    #endregion Private Members

    /// <summary>
    /// Gets the context.
    /// </summary>
    public DbContext Context { get; }

    #region Constructor / Destructor

    /// <summary>
    /// Initializes a new instance of the MappedEntityFrameworkRepositoryConnection class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="entityModelMapper">The entity model mapper.</param>
    /// <param name="isContextOwner">The is context owner.</param>
    public MappedEntityFrameworkRepositoryConnection(
        DbContext context,
        IEntityModelMapper<TEntity, TModel> entityModelMapper,
        bool isContextOwner)
    {
        this.Context = context;
        this.entityModelMapper = entityModelMapper;
        this.isContextOwner = isContextOwner;
    }

    /// <summary>
    /// Releases resources used by the MappedEntityFrameworkRepositoryConnection class.
    /// </summary>
    ~MappedEntityFrameworkRepositoryConnection()
    {
        Dispose(false);
    }

    #endregion Constructor / Destructor

    #region IRepositoryConnection<TEntity> Members

    /// <summary>
    /// Queries the value.
    /// </summary>
    /// <param name="includePaths">The include paths.</param>
    /// <returns>The t i queryable.</returns>
    public virtual IQueryable<TModel> Query(params Expression<Func<TModel, dynamic>>[] includePaths)
    {
        var query = Context.Set<TEntity>().AsNoTracking();

        if (!includePaths.IsNullOrEmpty())
        {
            var mappedIncludes = includePaths.Select(entityModelMapper.MapInclude).ToArray();
            foreach (var mappedInclude in mappedIncludes)
            {
                query = query.Include(mappedInclude);
            }
        }

        return entityModelMapper.MapQuery(query);
    }

    /// <summary>
    /// Queries the value.
    /// </summary>
    /// <param name="predicate">The predicate to apply.</param>
    /// <param name="includePaths">The include paths.</param>
    /// <returns>The t i queryable.</returns>
    public virtual IQueryable<TModel> Query(Expression<Func<TModel, bool>> predicate, params Expression<Func<TModel, dynamic>>[] includePaths)
    {
        var mappedPredicate = entityModelMapper.MapPredicate(predicate);
        var query = Context.Set<TEntity>().AsNoTracking().Where(mappedPredicate);

        if (!includePaths.IsNullOrEmpty())
        {
            var mappedIncludes = includePaths.Select(entityModelMapper.MapInclude).ToArray();
            foreach (var mappedInclude in mappedIncludes)
            {
                query = query.Include(mappedInclude);
            }
        }

        return entityModelMapper.MapQuery(query);
    }

    #endregion IRepositoryConnection<TEntity> Members

    #region IDisposable Members

    /// <summary>
    /// Releases the resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the resources used by this instance.
    /// </summary>
    /// <param name="disposing">The disposing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!isContextOwner)
        {
            return;
        }

        if (disposed)
        {
            return;
        }

        if (disposing)
        {
            Context?.Dispose();
        }

        disposed = true;
    }

    #endregion IDisposable Members
}
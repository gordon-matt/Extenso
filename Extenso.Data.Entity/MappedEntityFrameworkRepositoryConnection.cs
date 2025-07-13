using System.Linq.Expressions;
using Extenso.Collections;
using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity;

public class MappedEntityFrameworkRepositoryConnection<TEntity, TModel> : IEntityFrameworkRepositoryConnection<TModel>
    where TEntity : class
{
    #region Private Members

    private readonly bool isContextOwner;
    private bool disposed;

    private readonly IEntityModelMapper<TEntity, TModel> entityModelMapper;

    #endregion Private Members

    public DbContext Context { get; }

    #region Constructor / Destructor

    public MappedEntityFrameworkRepositoryConnection(
        DbContext context,
        IEntityModelMapper<TEntity, TModel> entityModelMapper,
        bool isContextOwner)
    {
        this.Context = context;
        this.entityModelMapper = entityModelMapper;
        this.isContextOwner = isContextOwner;
    }

    ~MappedEntityFrameworkRepositoryConnection()
    {
        Dispose(false);
    }

    #endregion Constructor / Destructor

    #region IRepositoryConnection<TEntity> Members

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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

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
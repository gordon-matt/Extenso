using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity;

public class EntityFrameworkRepositoryConnection<TEntity> : IEntityFrameworkRepositoryConnection<TEntity>
    where TEntity : class
{
    #region Private Members

    private readonly bool isContextOwner;
    private bool disposed;

    #endregion Private Members

    public DbContext Context { get; }

    #region Constructor / Destructor

    public EntityFrameworkRepositoryConnection(DbContext context, bool isContextOwner)
    {
        this.Context = context;
        this.isContextOwner = isContextOwner;
    }

    ~EntityFrameworkRepositoryConnection()
    {
        Dispose(false);
    }

    #endregion Constructor / Destructor

    #region IRepositoryConnection<TEntity> Members

    public virtual IQueryable<TEntity> Query(params Expression<Func<TEntity, dynamic>>[] includePaths)
    {
        var query = Context.Set<TEntity>().AsNoTracking();

        foreach (var path in includePaths)
        {
            query = query.Include(path);
        }

        return query;
    }

    public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] includePaths)
    {
        var query = Context.Set<TEntity>().AsNoTracking().Where(predicate);

        foreach (var path in includePaths)
        {
            query = query.Include(path);
        }

        return query;
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
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity;

public class EntityFrameworkRepositoryConnection<TEntity> : IRepositoryConnection<TEntity>
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

    public virtual IQueryable<TEntity> Query() => Context.Set<TEntity>().AsNoTracking();

    public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filterExpression) => Context.Set<TEntity>().AsNoTracking().Where(filterExpression);

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
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity;

public class MappedEntityFrameworkRepositoryConnection<TEntity, TModel> : IEntityFrameworkRepositoryConnection<TModel>
    where TEntity : class
{
    #region Private Members

    private readonly bool isContextOwner;
    private bool disposed;

    private readonly Func<IQueryable<TEntity>, IQueryable<TModel>> queryMapper;
    private readonly Func<Expression<Func<TModel, bool>>, Expression<Func<TEntity, bool>>> predicateMapper;

    #endregion Private Members

    public DbContext Context { get; }

    #region Constructor / Destructor

    public MappedEntityFrameworkRepositoryConnection(
        DbContext context,
        bool isContextOwner,
        Func<IQueryable<TEntity>, IQueryable<TModel>> queryMapper,
        Func<Expression<Func<TModel, bool>>, Expression<Func<TEntity, bool>>> predicateMapper)
    {
        this.Context = context;
        this.isContextOwner = isContextOwner;
        this.queryMapper = queryMapper;
        this.predicateMapper = predicateMapper;
    }

    ~MappedEntityFrameworkRepositoryConnection()
    {
        Dispose(false);
    }

    #endregion Constructor / Destructor

    #region IRepositoryConnection<TEntity> Members

    public virtual IQueryable<TModel> Query()
    {
        var query = Context.Set<TEntity>().AsNoTracking();
        var mappedQuery = queryMapper(query);
        return mappedQuery;
    }

    public virtual IQueryable<TModel> Query(Expression<Func<TModel, bool>> filterExpression)
    {
        var mappedPredicate = predicateMapper(filterExpression);
        var query = Context.Set<TEntity>().AsNoTracking().Where(mappedPredicate);
        var mappedQuery = queryMapper(query);
        return mappedQuery;
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
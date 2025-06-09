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

    private readonly Func<IQueryable<TEntity>, IQueryable<TModel>> queryMapper;
    private readonly Func<Expression<Func<TModel, bool>>, Expression<Func<TEntity, bool>>> predicateMapper;
    private readonly Func<Expression<Func<TModel, dynamic>>, Expression<Func<TEntity, dynamic>>> includeMapper;

    #endregion Private Members

    public DbContext Context { get; }

    #region Constructor / Destructor

    public MappedEntityFrameworkRepositoryConnection(
        DbContext context,
        bool isContextOwner,
        Func<IQueryable<TEntity>, IQueryable<TModel>> queryMapper,
        Func<Expression<Func<TModel, bool>>, Expression<Func<TEntity, bool>>> predicateMapper,
        Func<Expression<Func<TModel, dynamic>>, Expression<Func<TEntity, dynamic>>> includeMapper)
    {
        this.Context = context;
        this.isContextOwner = isContextOwner;
        this.queryMapper = queryMapper;
        this.predicateMapper = predicateMapper;
        this.includeMapper = includeMapper;
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
            var mappedIncludes = includePaths.Select(includeMapper).ToArray();
            foreach (var mappedInclude in mappedIncludes)
            {
                query = query.Include(mappedInclude);
            }
        }

        return queryMapper(query);
    }

    public virtual IQueryable<TModel> Query(Expression<Func<TModel, bool>> predicate, params Expression<Func<TModel, dynamic>>[] includePaths)
    {
        var mappedPredicate = predicateMapper(predicate);
        var query = Context.Set<TEntity>().AsNoTracking().Where(mappedPredicate);

        if (!includePaths.IsNullOrEmpty())
        {
            var mappedIncludes = includePaths.Select(includeMapper).ToArray();
            foreach (var mappedInclude in mappedIncludes)
            {
                query = query.Include(mappedInclude);
            }
        }

        return queryMapper(query);
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
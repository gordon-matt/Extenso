using System.Linq.Expressions;

namespace Extenso.Data.Entity;

public interface IRepositoryConnection<TEntity> : IDisposable
{
    IQueryable<TEntity> Query(params Expression<Func<TEntity, dynamic>>[] includePaths);

    IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filterExpression, params Expression<Func<TEntity, dynamic>>[] includePaths);
}
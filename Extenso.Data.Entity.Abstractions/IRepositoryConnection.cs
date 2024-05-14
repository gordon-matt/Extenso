using System;
using System.Linq;
using System.Linq.Expressions;

namespace Extenso.Data.Entity
{
    public interface IRepositoryConnection<TEntity> : IDisposable
    {
        IQueryable<TEntity> Query();

        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filterExpression);
    }
}
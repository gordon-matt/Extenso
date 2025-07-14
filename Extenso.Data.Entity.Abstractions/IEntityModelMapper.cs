using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Extenso.Data.Entity;

public interface IEntityModelMapper<TEntity, TModel>
{
    Func<IQueryable<TEntity>, IQueryable<TEntity>> MapInclude(Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> includeExpression);

    Expression<Func<TEntity, TProperty>> MapInclude<TProperty>(Expression<Func<TModel, TProperty>> includeExpression);

    Func<IQueryable<TEntity>, IQueryable<TEntity>> MapOrderBy(Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> includeExpression);

    Expression<Func<TEntity, bool>> MapPredicate(Expression<Func<TModel, bool>> predicate);

    Expression<Func<TEntity, TResult>> MapProjection<TResult>(Expression<Func<TModel, TResult>> projectionExpression);

    IQueryable<TModel> MapQuery(IQueryable<TEntity> query);

    Expression<Func<TEntity, TEntity>> MapUpdate(Expression<Func<TModel, TModel>> updateExpression);

    /// <summary>
    /// Maps SetPropertyCalls from TModel to TEntity for use with ExecuteUpdate operations.
    /// </summary>
    /// <param name="setPropertyCalls">The SetPropertyCalls expression for TModel</param>
    /// <returns>A SetPropertyCalls expression for TEntity</returns>
    Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> MapSetPropertyCalls(
        Expression<Func<SetPropertyCalls<TModel>, SetPropertyCalls<TModel>>> setPropertyCalls);

    TEntity ToEntity(TModel model);

    TModel ToModel(TEntity entity);
}
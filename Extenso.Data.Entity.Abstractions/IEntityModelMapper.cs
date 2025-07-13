using System.Linq.Expressions;

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

    TEntity ToEntity(TModel model);

    TModel ToModel(TEntity entity);
}
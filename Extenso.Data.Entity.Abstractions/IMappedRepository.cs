using System.Linq.Expressions;

namespace Extenso.Data.Entity;

public interface IMappedRepository<TModel, TEntity> : IRepository<TModel>
    where TModel : class
    where TEntity : class, IEntity
{
    TEntity ToEntity(TModel model);

    TModel ToModel(TEntity entity);

    IQueryable<TModel> MapQuery(IQueryable<TEntity> query);

    Expression<Func<TEntity, bool>> MapPredicate(Expression<Func<TModel, bool>> predicate);

    Expression<Func<TEntity, TProperty>> MapInclude<TProperty>(Expression<Func<TModel, TProperty>> includeExpression);

    Expression<Func<TEntity, TEntity>> MapUpdate(Expression<Func<TModel, TModel>> updateExpression);
}
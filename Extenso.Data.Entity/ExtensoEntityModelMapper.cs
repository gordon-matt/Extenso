using System.Linq.Expressions;
using Extenso.Mapping;

namespace Extenso.Data.Entity;

public class ExtensoEntityModelMapper<TEntity, TModel> : IEntityModelMapper<TEntity, TModel>
{
    public Expression<Func<TEntity, TProperty>> MapInclude<TProperty>(Expression<Func<TModel, TProperty>> includeExpression) =>
        ExtensoMapper.MapInclude<TModel, TEntity, TProperty>(includeExpression);

    public Func<IQueryable<TEntity>, IQueryable<TEntity>> MapInclude(
        Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> includeExpression) =>
        ExtensoMapper.MapInclude<TModel, TEntity>(includeExpression);

    public Func<IQueryable<TEntity>, IQueryable<TEntity>> MapOrderBy(
        Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> includeExpression) =>
        ExtensoMapper.MapOrderBy<TModel, TEntity>(includeExpression);

    public Expression<Func<TEntity, bool>> MapPredicate(Expression<Func<TModel, bool>> predicate) =>
        ExtensoMapper.MapPredicate<TModel, TEntity>(predicate);

    public Expression<Func<TEntity, TResult>> MapProjection<TResult>(
        Expression<Func<TModel, TResult>> projectionExpression) =>
        ExtensoMapper.MapProjection<TModel, TEntity, TResult>(projectionExpression);

    public IQueryable<TModel> MapQuery(IQueryable<TEntity> query) =>
        ExtensoMapper.MapQuery<TEntity, TModel>(query);

    public Expression<Func<TEntity, TEntity>> MapUpdate(Expression<Func<TModel, TModel>> updateExpression) =>
        ExtensoMapper.MapUpdate<TModel, TEntity>(updateExpression);

    public TEntity ToEntity(TModel model) => ExtensoMapper.Map<TModel, TEntity>(model);

    public TModel ToModel(TEntity entity) => ExtensoMapper.Map<TEntity, TModel>(entity);
}
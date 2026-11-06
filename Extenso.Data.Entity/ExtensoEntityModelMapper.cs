using System.Linq.Expressions;
using Extenso.Mapping;
using Microsoft.EntityFrameworkCore.Query;

namespace Extenso.Data.Entity;

/// <summary>
/// Represents the extenso entity model mapper.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TModel">The type of the model.</typeparam>
public class ExtensoEntityModelMapper<TEntity, TModel> : IEntityModelMapper<TEntity, TModel>
{
    /// <summary>
    /// Maps the include.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="includeExpression">The include expression.</param>
    /// <returns>The include.</returns>
    public Expression<Func<TEntity, TProperty>> MapInclude<TProperty>(Expression<Func<TModel, TProperty>> includeExpression) =>
        ExtensoMapper.MapInclude<TModel, TEntity, TProperty>(includeExpression);

    /// <summary>
    /// Maps the include.
    /// </summary>
    /// <param name="includeExpression">The include expression.</param>
    /// <returns>The include.</returns>
    public Func<IQueryable<TEntity>, IQueryable<TEntity>> MapInclude(
        Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> includeExpression) =>
        ExtensoMapper.MapInclude<TModel, TEntity>(includeExpression);

    /// <summary>
    /// Maps the order by.
    /// </summary>
    /// <param name="includeExpression">The include expression.</param>
    /// <returns>The order.</returns>
    public Func<IQueryable<TEntity>, IQueryable<TEntity>> MapOrderBy(
        Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> includeExpression) =>
        ExtensoMapper.MapOrderBy<TModel, TEntity>(includeExpression);

    /// <summary>
    /// Maps the predicate.
    /// </summary>
    /// <param name="predicate">The predicate to apply.</param>
    /// <returns>The predicate.</returns>
    public Expression<Func<TEntity, bool>> MapPredicate(Expression<Func<TModel, bool>> predicate) =>
        ExtensoMapper.MapPredicate<TModel, TEntity>(predicate);

    /// <summary>
    /// Maps the projection.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="projectionExpression">The projection expression.</param>
    /// <returns>The projection.</returns>
    public Expression<Func<TEntity, TResult>> MapProjection<TResult>(
        Expression<Func<TModel, TResult>> projectionExpression) =>
        ExtensoMapper.MapProjection<TModel, TEntity, TResult>(projectionExpression);

    /// <summary>
    /// Maps the query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <returns>The query.</returns>
    public IQueryable<TModel> MapQuery(IQueryable<TEntity> query) =>
        ExtensoMapper.MapQuery<TEntity, TModel>(query);

    /// <summary>
    /// Maps the update.
    /// </summary>
    /// <param name="updateExpression">The update expression.</param>
    /// <returns>The update.</returns>
    public Expression<Func<TEntity, TEntity>> MapUpdate(Expression<Func<TModel, TModel>> updateExpression) =>
        ExtensoMapper.MapUpdate<TModel, TEntity>(updateExpression);

    /// <summary>
    /// Maps the set property calls.
    /// </summary>
    /// <param name="setPropertyCalls">The set property calls.</param>
    /// <returns>The set property calls.</returns>
    public Expression<Action<UpdateSettersBuilder<TEntity>>> MapSetPropertyCalls(
        Expression<Action<UpdateSettersBuilder<TModel>>> setPropertyCalls) =>
        ExtensoMapper.MapSetPropertyCalls<TModel, TEntity>(setPropertyCalls);

    /// <summary>
    /// Converts the value to the entity.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>The entity.</returns>
    public TEntity ToEntity(TModel model) => ExtensoMapper.Map<TModel, TEntity>(model);

    /// <summary>
    /// Converts the value to the model.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>The model.</returns>
    public TModel ToModel(TEntity entity) => ExtensoMapper.Map<TEntity, TModel>(entity);
}
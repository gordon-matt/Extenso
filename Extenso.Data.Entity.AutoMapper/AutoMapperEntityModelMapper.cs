using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore.Query;

namespace Extenso.Data.Entity.AutoMapper;

/// <summary>
/// Represents the auto mapper entity model mapper.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TModel">The type of the model.</typeparam>
public class AutoMapperEntityModelMapper<TEntity, TModel> : IEntityModelMapper<TEntity, TModel>
{
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the AutoMapperEntityModelMapper class.
    /// </summary>
    /// <param name="mapper">The mapper.</param>
    public AutoMapperEntityModelMapper(IMapper mapper)
    {
        this.mapper = mapper;
    }

    /// <summary>
    /// Maps the include.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="includeExpression">The include expression.</param>
    /// <returns>The include.</returns>
    public Expression<Func<TEntity, TProperty>> MapInclude<TProperty>(Expression<Func<TModel, TProperty>> includeExpression) =>
        mapper.MapExpressionAsInclude<Expression<Func<TEntity, TProperty>>>(includeExpression);

    /// <summary>
    /// Maps the include.
    /// </summary>
    /// <param name="includeExpression">The include expression.</param>
    /// <returns>The include.</returns>
    public Func<IQueryable<TEntity>, IQueryable<TEntity>> MapInclude(
        Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> includeExpression) =>
        mapper.MapExpressionAsInclude<TModel, TEntity>(includeExpression);

    /// <summary>
    /// Maps the order by.
    /// </summary>
    /// <param name="orderByExpression">The order by expression.</param>
    /// <returns>The order.</returns>
    public Func<IQueryable<TEntity>, IQueryable<TEntity>> MapOrderBy(
        Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> orderByExpression) =>
        mapper.MapExpressionAsOrderBy<TModel, TEntity>(orderByExpression);

    /// <summary>
    /// Maps the predicate.
    /// </summary>
    /// <param name="predicate">The predicate to apply.</param>
    /// <returns>The predicate.</returns>
    public Expression<Func<TEntity, bool>> MapPredicate(Expression<Func<TModel, bool>> predicate) =>
        mapper.MapExpression<Expression<Func<TEntity, bool>>>(predicate);

    /// <summary>
    /// Maps the projection.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="projectionExpression">The projection expression.</param>
    /// <returns>The projection.</returns>
    public Expression<Func<TEntity, TResult>> MapProjection<TResult>(
        Expression<Func<TModel, TResult>> projectionExpression) =>
        mapper.MapExpression<Expression<Func<TEntity, TResult>>>(projectionExpression);

    /// <summary>
    /// Maps the query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <returns>The query.</returns>
    public IQueryable<TModel> MapQuery(IQueryable<TEntity> query) =>
        mapper.ProjectTo<TModel>(query);

    /// <summary>
    /// Maps the update.
    /// </summary>
    /// <param name="updateExpression">The update expression.</param>
    /// <returns>The update.</returns>
    public Expression<Func<TEntity, TEntity>> MapUpdate(Expression<Func<TModel, TModel>> updateExpression) =>
        mapper.MapExpression<Expression<Func<TEntity, TEntity>>>(updateExpression);

    /// <summary>
    /// Maps the set property calls.
    /// </summary>
    /// <param name="setPropertyCalls">The set property calls.</param>
    /// <returns>The set property calls.</returns>
    public Expression<Action<UpdateSettersBuilder<TEntity>>> MapSetPropertyCalls(
        Expression<Action<UpdateSettersBuilder<TModel>>> setPropertyCalls) =>
        mapper.MapExpression<Expression<Action<UpdateSettersBuilder<TEntity>>>>(setPropertyCalls);

    /// <summary>
    /// Converts the value to the entity.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>The entity.</returns>
    public TEntity ToEntity(TModel model) => mapper.Map<TModel, TEntity>(model);

    /// <summary>
    /// Converts the value to the model.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>The model.</returns>
    public TModel ToModel(TEntity entity) => mapper.Map<TEntity, TModel>(entity);
}
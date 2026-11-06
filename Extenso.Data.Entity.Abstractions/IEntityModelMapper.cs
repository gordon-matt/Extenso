using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Extenso.Data.Entity;

/// <summary>
/// Defines the contract for the entity model mapper.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TModel">The type of the model.</typeparam>
public interface IEntityModelMapper<TEntity, TModel>
{
    /// <summary>
    /// Maps the include.
    /// </summary>
    /// <param name="includeExpression">The include expression.</param>
    /// <returns>The include.</returns>
    Func<IQueryable<TEntity>, IQueryable<TEntity>> MapInclude(Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> includeExpression);

    /// <summary>
    /// Maps the include.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="includeExpression">The include expression.</param>
    /// <returns>The include.</returns>
    Expression<Func<TEntity, TProperty>> MapInclude<TProperty>(Expression<Func<TModel, TProperty>> includeExpression);

    /// <summary>
    /// Maps the order by.
    /// </summary>
    /// <param name="includeExpression">The include expression.</param>
    /// <returns>The order.</returns>
    Func<IQueryable<TEntity>, IQueryable<TEntity>> MapOrderBy(Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> includeExpression);

    /// <summary>
    /// Maps the predicate.
    /// </summary>
    /// <param name="predicate">The predicate to apply.</param>
    /// <returns>The predicate.</returns>
    Expression<Func<TEntity, bool>> MapPredicate(Expression<Func<TModel, bool>> predicate);

    /// <summary>
    /// Maps the projection.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="projectionExpression">The projection expression.</param>
    /// <returns>The projection.</returns>
    Expression<Func<TEntity, TResult>> MapProjection<TResult>(Expression<Func<TModel, TResult>> projectionExpression);

    /// <summary>
    /// Maps the query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <returns>The query.</returns>
    IQueryable<TModel> MapQuery(IQueryable<TEntity> query);

    /// <summary>
    /// Maps the update.
    /// </summary>
    /// <param name="updateExpression">The update expression.</param>
    /// <returns>The update.</returns>
    Expression<Func<TEntity, TEntity>> MapUpdate(Expression<Func<TModel, TModel>> updateExpression);

    /// <summary>
    /// Maps SetPropertyCalls from TModel to TEntity for use with ExecuteUpdate operations.
    /// </summary>
    /// <param name="setPropertyCalls">The SetPropertyCalls expression for TModel</param>
    /// <returns>A SetPropertyCalls expression for TEntity</returns>
    Expression<Action<UpdateSettersBuilder<TEntity>>> MapSetPropertyCalls(
        Expression<Action<UpdateSettersBuilder<TModel>>> setPropertyCalls);

    /// <summary>
    /// Converts the value to the entity.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>The entity.</returns>
    TEntity ToEntity(TModel model);

    /// <summary>
    /// Converts the value to the model.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>The model.</returns>
    TModel ToModel(TEntity entity);
}
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Extenso.Data.Entity;

public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
    where TEntity : class
{
    #region Delete

    /// <summary>
    /// Deletes all rows without retrieving entities.
    /// </summary>
    /// <returns>The number of rows affected.</returns>
    int DeleteAll(ContextOptions options = null);

    /// <summary>
    /// Deletes the given entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>The number of rows affected.</returns>
    int Delete(TEntity entity, ContextOptions options = null);

    /// <summary>
    /// Deletes the given entities.
    /// </summary>
    /// <param name="entities">The entities to delete.</param>
    /// <returns>The number of rows affected.</returns>
    int Delete(IEnumerable<TEntity> entities, ContextOptions options = null);

    /// <summary>
    /// Deletes all entities that match the given predicate
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>The number of rows affected.</returns>
    int Delete(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null);

    /// <summary>
    /// Asynchronously deletes all rows without retrieving entities.
    /// </summary>
    /// <returns>A task with the number of rows affected.</returns>
    Task<int> DeleteAllAsync(ContextOptions options = null);

    /// <summary>
    /// Asynchronously deletes the given entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task with the number of rows affected.</returns>
    Task<int> DeleteAsync(TEntity entity, ContextOptions options = null);

    /// <summary>
    /// Asynchronously deletes the given entities
    /// </summary>
    /// <param name="entities">The entities to delete.</param>
    /// <returns>A task with the number of rows affected.</returns>
    Task<int> DeleteAsync(IEnumerable<TEntity> entities, ContextOptions options = null);

    /// <summary>
    /// Asynchronously deletes all entities that match the given predicate
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A task with the number of rows affected.</returns>
    Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null);

    #endregion Delete

    #region Insert

    /// <summary>
    /// Inserts the given entity.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>The number of rows affected.</returns>
    TEntity Insert(TEntity entity, ContextOptions options = null);

    /// <summary>
    /// Inserts the given entities.
    /// </summary>
    /// <param name="entities">The entities to insert.</param>
    /// <returns>The number of rows affected.</returns>
    IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities, ContextOptions options = null);

    /// <summary>
    /// Asynchronously inserts the given entity.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>A task with the number of rows affected.</returns>
    Task<TEntity> InsertAsync(TEntity entity, ContextOptions options = null);

    /// <summary>
    /// Asynchronously inserts the given entities.
    /// </summary>
    /// <param name="entities">The entities to insert.</param>
    /// <returns>A task with the number of rows affected.</returns>
    Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities, ContextOptions options = null);

    #endregion Insert

    #region Update

    /// <summary>
    /// Updates the given entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>The number of rows affected.</returns>
    TEntity Update(TEntity entity, ContextOptions options = null);

    /// <summary>
    /// Updates the given entities.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    /// <returns>The number of rows affected.</returns>
    IEnumerable<TEntity> Update(IEnumerable<TEntity> entities, ContextOptions options = null);

    /// <summary>
    /// Asynchronously updates the given entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task with the number of rows affected.</returns>
    Task<TEntity> UpdateAsync(TEntity entity, ContextOptions options = null);

    /// <summary>
    /// Asynchronously updates the given entities.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    /// <returns>A task with the number of rows affected.</returns>
    Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities, ContextOptions options = null);

    /// <summary>
    /// Updates all rows using SetPropertyCalls without retrieving entities.
    /// </summary>
    /// <param name="setPropertyCalls">The SetPropertyCalls expression.</param>
    /// <returns>The number of rows affected.</returns>
    int Update(Expression<Action<UpdateSettersBuilder<TEntity>>> setPropertyCalls, ContextOptions options = null);

    /// <summary>
    /// Updates all rows that match the given predicate using SetPropertyCalls without retrieving entities.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="setPropertyCalls">The SetPropertyCalls expression.</param>
    /// <returns>The number of rows affected.</returns>
    int Update(Expression<Func<TEntity, bool>> predicate, Expression<Action<UpdateSettersBuilder<TEntity>>> setPropertyCalls, ContextOptions options = null);

    /// <summary>
    /// Asynchronously updates all rows using SetPropertyCalls without retrieving entities.
    /// </summary>
    /// <param name="setPropertyCalls">The SetPropertyCalls expression.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> UpdateAsync(Expression<Action<UpdateSettersBuilder<TEntity>>> setPropertyCalls, ContextOptions options = null);

    /// <summary>
    /// Asynchronously updates all rows that match the given predicate using SetPropertyCalls without retrieving entities.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="setPropertyCalls">The SetPropertyCalls expression.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Action<UpdateSettersBuilder<TEntity>>> setPropertyCalls, ContextOptions options = null);

    #endregion Update
}
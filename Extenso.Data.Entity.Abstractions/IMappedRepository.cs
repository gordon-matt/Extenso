using System.Linq.Expressions;
using Extenso.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;

namespace Extenso.Data.Entity;

public interface IMappedRepository<TModel, TEntity> : IReadOnlyMappedRepository<TModel, TEntity>
    where TModel : class
    where TEntity : class, IEntity
{
    #region Delete

    /// <summary>
    /// Deletes the given entity.
    /// </summary>
    /// <param name="model">The entity to delete.</param>
    /// <returns>The number of rows affected.</returns>
    int Delete(TModel model, ContextOptions options = null);

    /// <summary>
    /// Deletes the given entities.
    /// </summary>
    /// <param name="models">The entities to delete.</param>
    /// <returns>The number of rows affected.</returns>
    int Delete(IEnumerable<TModel> models, ContextOptions options = null);

    /// <summary>
    /// Deletes all entities that match the given predicate
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>The number of rows affected.</returns>
    int Delete(Expression<Func<TModel, bool>> predicate, ContextOptions options = null);

    /// <summary>
    /// Deletes all rows without retrieving entities.
    /// </summary>
    /// <returns>The number of rows affected.</returns>
    int DeleteAll(ContextOptions options = null);

    /// <summary>
    /// Asynchronously deletes all rows without retrieving entities.
    /// </summary>
    /// <returns>A task with the number of rows affected.</returns>
    Task<int> DeleteAllAsync(ContextOptions options = null);

    /// <summary>
    /// Asynchronously deletes the given entity.
    /// </summary>
    /// <param name="model">The entity to delete.</param>
    /// <returns>A task with the number of rows affected.</returns>
    Task<int> DeleteAsync(TModel model, ContextOptions options = null);

    /// <summary>
    /// Asynchronously deletes the given entities
    /// </summary>
    /// <param name="models">The entities to delete.</param>
    /// <returns>A task with the number of rows affected.</returns>
    Task<int> DeleteAsync(IEnumerable<TModel> models, ContextOptions options = null);

    /// <summary>
    /// Asynchronously deletes all entities that match the given predicate
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A task with the number of rows affected.</returns>
    Task<int> DeleteAsync(Expression<Func<TModel, bool>> predicate, ContextOptions options = null);

    #endregion Delete

    #region Insert

    /// <summary>
    /// Inserts the given entity.
    /// </summary>
    /// <param name="model">The entity to insert.</param>
    /// <returns>The number of rows affected.</returns>
    TModel Insert(TModel model, ContextOptions options = null);

    /// <summary>
    /// Inserts the given entities.
    /// </summary>
    /// <param name="models">The entities to insert.</param>
    /// <returns>The number of rows affected.</returns>
    IEnumerable<TModel> Insert(IEnumerable<TModel> models, ContextOptions options = null);

    /// <summary>
    /// Asynchronously inserts the given entity.
    /// </summary>
    /// <param name="model">The entity to insert.</param>
    /// <returns>A task with the number of rows affected.</returns>
    Task<TModel> InsertAsync(TModel model, ContextOptions options = null);

    /// <summary>
    /// Asynchronously inserts the given entities.
    /// </summary>
    /// <param name="models">The entities to insert.</param>
    /// <returns>A task with the number of rows affected.</returns>
    Task<IEnumerable<TModel>> InsertAsync(IEnumerable<TModel> models, ContextOptions options = null);

    #endregion Insert

    #region Update

    /// <summary>
    /// Updates the given entity.
    /// </summary>
    /// <param name="model">The entity to update.</param>
    /// <returns>The number of rows affected.</returns>
    TModel Update(TModel model, ContextOptions options = null);

    /// <summary>
    /// Updates the given entities.
    /// </summary>
    /// <param name="models">The entities to update.</param>
    /// <returns>The number of rows affected.</returns>
    IEnumerable<TModel> Update(IEnumerable<TModel> models, ContextOptions options = null);

    /// <summary>
    /// Asynchronously updates the given entity.
    /// </summary>
    /// <param name="model">The entity to update.</param>
    /// <returns>A task with the number of rows affected.</returns>
    Task<TModel> UpdateAsync(TModel model, ContextOptions options = null);

    /// <summary>
    /// Asynchronously updates the given entities.
    /// </summary>
    /// <param name="models">The entities to update.</param>
    /// <returns>A task with the number of rows affected.</returns>
    Task<IEnumerable<TModel>> UpdateAsync(IEnumerable<TModel> models, ContextOptions options = null);

    /// <summary>
    /// Updates all rows using SetPropertyCalls without retrieving entities.
    /// </summary>
    /// <param name="setPropertyCalls">The SetPropertyCalls expression.</param>
    /// <returns>The number of rows affected.</returns>
    int Update(Expression<Action<UpdateSettersBuilder<TModel>>> setPropertyCalls, ContextOptions options = null);

    /// <summary>
    /// Updates all rows that match the given predicate using SetPropertyCalls without retrieving entities.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="setPropertyCalls">The SetPropertyCalls expression.</param>
    /// <returns>The number of rows affected.</returns>
    int Update(Expression<Func<TModel, bool>> predicate, Expression<Action<UpdateSettersBuilder<TModel>>> setPropertyCalls, ContextOptions options = null);

    /// <summary>
    /// Asynchronously updates all rows using SetPropertyCalls without retrieving entities.
    /// </summary>
    /// <param name="setPropertyCalls">The SetPropertyCalls expression.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> UpdateAsync(Expression<Action<UpdateSettersBuilder<TModel>>> setPropertyCalls, ContextOptions options = null);

    /// <summary>
    /// Asynchronously updates all rows that match the given predicate using SetPropertyCalls without retrieving entities.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="setPropertyCalls">The SetPropertyCalls expression.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> UpdateAsync(Expression<Func<TModel, bool>> predicate, Expression<Action<UpdateSettersBuilder<TModel>>> setPropertyCalls, ContextOptions options = null);

    #endregion Update
}
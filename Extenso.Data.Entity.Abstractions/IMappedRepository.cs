using System.Linq.Expressions;
using Extenso.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;

namespace Extenso.Data.Entity;

public interface IMappedRepository<TModel, TEntity>
    where TModel : class
    where TEntity : class, IEntity
{
    /// <summary>
    /// <para>This is typically used to create queries with custom projections. For example, you may wish</para>
    /// <para>to select only a small number of columns from the table.</para>
    /// </summary>
    /// <returns>An instance of IRepositoryConnectionlt;TEntity&gt;</returns>
    IRepositoryConnection<TModel> OpenConnection(ContextOptions options = null);

    /// <summary>
    /// <para>If OpenConnection() has already been called and you wish to use the same connection (same DbContext</para>
    /// <para>in Entity Framework), then use this.</para>
    /// </summary>
    /// <typeparam name="TOther">The type used in the other connection.</typeparam>
    /// <param name="connection">An instance of IRepositoryConnectionlt;TOther&gt;</param>
    /// <returns>An instance of IRepositoryConnectionlt;TEntity&gt;</returns>
    IRepositoryConnection<TModel> UseConnection<TOther>(IRepositoryConnection<TOther> connection)
        where TOther : class;

    #region Find

    /// <summary>
    ///  Finds all entities in the set.
    /// </summary>
    /// <param name="options"></param>
    /// <returns>A collection of all entities in the set.</returns>
    IPagedCollection<TModel> Find(SearchOptions<TEntity> options);

    /// <summary>
    ///  Finds all entities in the set.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="projection"></param>
    /// <returns>A collection of all entities in the set.</returns>
    IPagedCollection<TResult> Find<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection);

    /// <summary>
    ///  Finds all entities in the set.
    /// </summary>
    /// <param name="options"></param>
    /// <returns>A collection of all entities in the set.</returns>
    Task<IPagedCollection<TModel>> FindAsync(SearchOptions<TEntity> options);

    /// <summary>
    ///  Finds all entities in the set.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="projection"></param>
    /// <returns>A collection of all entities in the set.</returns>
    Task<IPagedCollection<TResult>> FindAsync<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection);

    /// <summary>
    ///  Finds an entity with the given primary key values.
    /// </summary>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>The entity found, or null.</returns>
    TModel FindOne(params object[] keyValues);

    /// <summary>
    /// Finds an entity based on a predicate.
    /// </summary>
    /// <param name="options"></param>
    /// <returns>The entity found, or null.</returns>
    TModel FindOne(SearchOptions<TEntity> options);

    /// <summary>
    /// Finds an entity based on a predicate.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="projection"></param>
    /// <returns>The entity found, or null.</returns>
    TResult FindOne<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection);

    /// <summary>
    ///  Asynchronously finds an entity with the given primary key values.
    /// </summary>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>The entity found, or null.</returns>
    Task<TModel> FindOneAsync(params object[] keyValues);

    /// <summary>
    /// Asynchronously finds an entity based on a predicate.
    /// </summary>
    /// <param name="options"></param>
    /// <returns>The entity found, or null.</returns>
    Task<TModel> FindOneAsync(SearchOptions<TEntity> options);

    /// <summary>
    /// Finds an entity based on a predicate.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="projection"></param>
    /// <returns>The entity found, or null.</returns>
    Task<TResult> FindOneAsync<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection);

    #endregion Find

    #region Count

    /// <summary>
    /// Returns the number of elements in a sequence.
    /// </summary>
    /// <returns>The number of elements in the sequence.</returns>
    int Count(ContextOptions options = null);

    /// <summary>
    /// Returns the number of elements in a sequence that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>The number of elements in the sequence that satisfy the condition in the predicate function.</returns>
    int Count(Expression<Func<TModel, bool>> predicate, ContextOptions options = null);

    /// <summary>
    /// Asynchronously returns the number of elements in a sequence.
    /// </summary>
    /// <returns>
    /// <para>A task that represents the asynchronous operation. The task result contains the number</para>
    /// <para>of elements in the sequence.</para>
    /// </returns>
    Task<int> CountAsync(ContextOptions options = null);

    /// <summary>
    /// Asynchronously returns the number of elements in a sequence that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>
    /// <para>A task that represents the asynchronous operation. The task result contains the number</para>
    /// <para>of elements in the sequence that satisfy the condition in the predicate function.</para>
    /// </returns>
    Task<int> CountAsync(Expression<Func<TModel, bool>> predicate, ContextOptions options = null);

    /// <summary>
    /// Returns a System.Int64 that represents the number of elements in a sequence.
    /// </summary>
    /// <returns>The number of elements in the sequence.</returns>
    long LongCount(ContextOptions options = null);

    /// <summary>
    /// Returns a System.Int64 that represents the number of elements in a sequence that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>The number of elements in the sequence that satisfy the condition in the predicate function.</returns>
    long LongCount(Expression<Func<TModel, bool>> predicate, ContextOptions options = null);

    /// <summary>
    /// Asynchronously returns a System.Int64 that represents the number of elements in a sequence.
    /// </summary>
    /// <returns>
    /// <para>A task that represents the asynchronous operation. The task result contains the number</para>
    /// <para>of elements in the sequence.</para>
    /// </returns>
    Task<long> LongCountAsync(ContextOptions options = null);

    /// <summary>
    /// Asynchronously returns a System.Int64 that represents the number of elements in a sequence that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>
    /// <para>A task that represents the asynchronous operation. The task result contains the number</para>
    /// <para>of elements in the sequence that satisfy the condition in the predicate function.</para>
    /// </returns>
    Task<long> LongCountAsync(Expression<Func<TModel, bool>> predicate, ContextOptions options = null);

    #endregion Count

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
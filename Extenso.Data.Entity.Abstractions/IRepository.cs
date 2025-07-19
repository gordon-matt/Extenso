using System.Linq.Expressions;
using Extenso.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;

namespace Extenso.Data.Entity;

public interface IRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    /// <para>This is typically used to create queries with custom projections. For example, you may wish</para>
    /// <para>to select only a small number of columns from the table.</para>
    /// </summary>
    /// <returns>An instance of IRepositoryConnectionlt;TEntity&gt;</returns>
    IRepositoryConnection<TEntity> OpenConnection(ContextOptions options = null);

    /// <summary>
    /// <para>If OpenConnection() has already been called and you wish to use the same connection (same DbContext</para>
    /// <para>in Entity Framework), then use this.</para>
    /// </summary>
    /// <typeparam name="TOther">The type used in the other connection.</typeparam>
    /// <param name="connection">An instance of IRepositoryConnectionlt;TOther&gt;</param>
    /// <returns>An instance of IRepositoryConnectionlt;TEntity&gt;</returns>
    IRepositoryConnection<TEntity> UseConnection<TOther>(IRepositoryConnection<TOther> connection)
        where TOther : class;

    #region Find

    /// <summary>
    ///  Finds all entities in the set.
    /// </summary>
    /// <param name="options"></param>
    /// <returns>A collection of all entities in the set.</returns>
    IPagedCollection<TEntity> Find(SearchOptions<TEntity> options);

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
    Task<IPagedCollection<TEntity>> FindAsync(SearchOptions<TEntity> options);

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
    TEntity FindOne(params object[] keyValues);

    /// <summary>
    /// Finds an entity based on a predicate.
    /// </summary>
    /// <param name="options"></param>
    /// <returns>The entity found, or null.</returns>
    TEntity FindOne(SearchOptions<TEntity> options);

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
    Task<TEntity> FindOneAsync(params object[] keyValues);

    /// <summary>
    /// Asynchronously finds an entity based on a predicate.
    /// </summary>
    /// <param name="options"></param>
    /// <returns>The entity found, or null.</returns>
    Task<TEntity> FindOneAsync(SearchOptions<TEntity> options);

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
    int Count(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null);

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
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null);

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
    long LongCount(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null);

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
    Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null);

    #endregion Count

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
    int Update(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, ContextOptions options = null);

    /// <summary>
    /// Updates all rows that match the given predicate using SetPropertyCalls without retrieving entities.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="setPropertyCalls">The SetPropertyCalls expression.</param>
    /// <returns>The number of rows affected.</returns>
    int Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, ContextOptions options = null);

    /// <summary>
    /// Asynchronously updates all rows using SetPropertyCalls without retrieving entities.
    /// </summary>
    /// <param name="setPropertyCalls">The SetPropertyCalls expression.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> UpdateAsync(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, ContextOptions options = null);

    /// <summary>
    /// Asynchronously updates all rows that match the given predicate using SetPropertyCalls without retrieving entities.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="setPropertyCalls">The SetPropertyCalls expression.</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, ContextOptions options = null);

    #endregion Update
}
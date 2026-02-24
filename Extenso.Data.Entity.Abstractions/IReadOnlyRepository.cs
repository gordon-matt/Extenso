using System.Linq.Expressions;
using Extenso.Collections.Generic;

namespace Extenso.Data.Entity;

public interface IReadOnlyRepository<TEntity>
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

    IAsyncEnumerable<TEntity> StreamAsync(
        SearchOptions<TEntity> options,
        CancellationToken cancellationToken = default);

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

    /// <summary>
    /// Determines whether any element of a sequence satisfies a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>true if any elements in the sequence satisfy the condition in the predicate function; otherwise, false.</returns>
    bool Exists(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null);

    /// <summary>
    /// Asynchronously determines whether any element of a sequence satisfies a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>
    /// <para>A task that represents the asynchronous operation. The task result contains true if any elements</para>
    /// <para>in the sequence satisfy the condition in the predicate function; otherwise, false.</para>
    /// </returns>
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null);
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Extenso.Data.Entity
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// <para>This is typically used to create queries with custom projections. For example, you may wish</para>
        /// <para>to select only a small number of columns from the table.</para>
        /// </summary>
        /// <returns>An instance of IRepositoryConnectionlt;TEntity&gt;</returns>
        IRepositoryConnection<TEntity> OpenConnection();

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
        /// <param name="includePaths">Specifies related entities to include in the query results.</param>
        /// <returns>A collection of all entities in the set.</returns>
        IEnumerable<TEntity> Find(params Expression<Func<TEntity, dynamic>>[] includePaths);

        /// <summary>
        /// Finds a filtered list of entities based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="includePaths">Specifies related entities to include in the query results.</param>
        /// <returns>A filtered list of entities based on a predicate.</returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] includePaths);

        /// <summary>
        /// Asynchronously finds all entities in the set.
        /// </summary>
        /// <param name="includePaths">Specifies related entities to include in the query results.</param>
        /// <returns>A collection of all entities in the set.</returns>
        Task<IEnumerable<TEntity>> FindAsync(params Expression<Func<TEntity, dynamic>>[] includePaths);

        /// <summary>
        /// Asynchronously finds a filtered list of entities based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="includePaths">Specifies related entities to include in the query results.</param>
        /// <returns>A filtered list of entities based on a predicate.</returns>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] includePaths);

        /// <summary>
        ///  Finds an entity with the given primary key values.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>The entity found, or null.</returns>
        TEntity FindOne(params object[] keyValues);

        /// <summary>
        /// Finds an entity based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="includePaths">Specifies related entities to include in the query results.</param>
        /// <returns>The entity found, or null.</returns>
        TEntity FindOne(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] includePaths);

        /// <summary>
        ///  Asynchronously finds an entity with the given primary key values.
        /// </summary>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>The entity found, or null.</returns>
        Task<TEntity> FindOneAsync(params object[] keyValues);

        /// <summary>
        /// Asynchronously finds an entity based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="includePaths">Specifies related entities to include in the query results.</param>
        /// <returns>The entity found, or null.</returns>
        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, dynamic>>[] includePaths);

        #endregion Find

        #region Count

        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        /// <returns>The number of elements in the sequence.</returns>
        int Count();

        /// <summary>
        /// Returns the number of elements in a sequence that satisfy a condition.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The number of elements in the sequence that satisfy the condition in the predicate function.</returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Asynchronously returns the number of elements in a sequence.
        /// </summary>
        /// <returns>
        /// <para>A task that represents the asynchronous operation. The task result contains the number</para>
        /// <para>of elements in the sequence.</para>
        /// </returns>
        Task<int> CountAsync();

        /// <summary>
        /// Asynchronously returns the number of elements in a sequence that satisfy a condition.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// <para>A task that represents the asynchronous operation. The task result contains the number</para>
        /// <para>of elements in the sequence that satisfy the condition in the predicate function.</para>
        /// </returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Returns a System.Int64 that represents the number of elements in a sequence.
        /// </summary>
        /// <returns>The number of elements in the sequence.</returns>
        long LongCount();

        /// <summary>
        /// Returns a System.Int64 that represents the number of elements in a sequence that satisfy a condition.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The number of elements in the sequence that satisfy the condition in the predicate function.</returns>
        long LongCount(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Asynchronously returns a System.Int64 that represents the number of elements in a sequence.
        /// </summary>
        /// <returns>
        /// <para>A task that represents the asynchronous operation. The task result contains the number</para>
        /// <para>of elements in the sequence.</para>
        /// </returns>
        Task<long> LongCountAsync();

        /// <summary>
        /// Asynchronously returns a System.Int64 that represents the number of elements in a sequence that satisfy a condition.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// <para>A task that represents the asynchronous operation. The task result contains the number</para>
        /// <para>of elements in the sequence that satisfy the condition in the predicate function.</para>
        /// </returns>
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion Count

        #region Delete

        /// <summary>
        /// Deletes all rows without retrieving entities.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        int DeleteAll();

        /// <summary>
        /// Deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>The number of rows affected.</returns>
        int Delete(TEntity entity);

        /// <summary>
        /// Deletes the given entities.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <returns>The number of rows affected.</returns>
        int Delete(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes all entities that match the given predicate
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The number of rows affected.</returns>
        int Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Asynchronously deletes all rows without retrieving entities.
        /// </summary>
        /// <returns>A task with the number of rows affected.</returns>
        Task<int> DeleteAllAsync();

        /// <summary>
        /// Asynchronously deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task with the number of rows affected.</returns>
        Task<int> DeleteAsync(TEntity entity);

        /// <summary>
        /// Asynchronously deletes the given entities
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <returns>A task with the number of rows affected.</returns>
        Task<int> DeleteAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Asynchronously deletes all entities that match the given predicate
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A task with the number of rows affected.</returns>
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion Delete

        #region Insert

        /// <summary>
        /// Inserts the given entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>The number of rows affected.</returns>
        int Insert(TEntity entity);

        /// <summary>
        /// Inserts the given entities.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <returns>The number of rows affected.</returns>
        int Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Asynchronously inserts the given entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>A task with the number of rows affected.</returns>
        Task<int> InsertAsync(TEntity entity);

        /// <summary>
        /// Asynchronously inserts the given entities.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        /// <returns>A task with the number of rows affected.</returns>
        Task<int> InsertAsync(IEnumerable<TEntity> entities);

        #endregion Insert

        #region Update

        /// <summary>
        /// Updates the given entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The number of rows affected.</returns>
        int Update(TEntity entity);

        /// <summary>
        /// Updates the given entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <returns>The number of rows affected.</returns>
        int Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// Asynchronously updates the given entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task with the number of rows affected.</returns>
        Task<int> UpdateAsync(TEntity entity);

        /// <summary>
        /// Asynchronously updates the given entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <returns>A task with the number of rows affected.</returns>
        Task<int> UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates all rows using an expression without retrieving entities.
        /// </summary>
        /// <param name="updateFactory">The update expression.</param>
        /// <returns>The number of rows affected.</returns>
        int Update(Expression<Func<TEntity, TEntity>> updateFactory);

        /// <summary>
        /// Updates all rows that match the given predicate using an expression without retrieving entities.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="updateFactory">The update expression.</param>
        /// <returns>The number of rows affected.</returns>
        int Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateFactory);

        /// <summary>
        /// Updates all rows from the query using an expression without retrieving entities.
        /// </summary>
        /// <param name="query">The query to update rows from without retrieving entities.</param>
        /// <param name="updateFactory">The update expression.</param>
        /// <returns>The number of rows affected.</returns>
        int Update(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateFactory);

        /// <summary>
        /// Asynchronously updates all rows using an expression without retrieving entities.
        /// </summary>
        /// <param name="updateFactory">The update expression.</param>
        /// <returns>The number of rows affected.</returns>
        Task<int> UpdateAsync(Expression<Func<TEntity, TEntity>> updateFactory);

        /// <summary>
        /// Asynchronously updates all rows that match the given predicate using an expression without retrieving entities.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="updateFactory">The update expression.</param>
        /// <returns>The number of rows affected.</returns>
        Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateFactory);

        /// <summary>
        /// Asynchronously updates all rows from the query using an expression without retrieving entities.
        /// </summary>
        /// <param name="query">The query to update rows from without retrieving entities.</param>
        /// <param name="updateFactory">The update expression.</param>
        /// <returns>The number of rows affected.</returns>
        Task<int> UpdateAsync(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateFactory);

        #endregion Update
    }
}
using System.Linq.Expressions;
using Extenso.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Z.EntityFramework.Plus;

namespace Extenso.Data.Entity;

public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity
{
    #region Non-Public Members

    protected IDbContextFactory contextFactory;
    private readonly ILogger logger;

    #endregion Non-Public Members

    #region Constructor

    public EntityFrameworkRepository(IDbContextFactory contextFactory, ILoggerFactory loggerFactory)
    {
        this.contextFactory = contextFactory;
        logger = loggerFactory.CreateLogger<EntityFrameworkRepository<TEntity>>();
    }

    #endregion Constructor

    #region IRepository<TEntity> Members

    /// <summary>
    /// <para>This is typically used to create queries with custom projections. For example, you may wish</para>
    /// <para>to select only a small number of columns from the table.</para>
    /// </summary>
    /// <returns>An instance of IRepositoryConnectionlt;TEntity&gt;</returns>
    public virtual IRepositoryConnection<TEntity> OpenConnection()
    {
        var context = GetContext();
        return new EntityFrameworkRepositoryConnection<TEntity>(context, true);
    }

    /// <summary>
    /// <para>If OpenConnection() has already been called and you wish to use the same connection</para>
    /// <para> (same DbContext), then use this.</para>
    /// </summary>
    /// <typeparam name="TOther">The type used in the other connection.</typeparam>
    /// <param name="connection">An instance of IRepositoryConnectionlt;TOther&gt;</param>
    /// <returns>An instance of IRepositoryConnectionlt;TEntity&gt;</returns>
    public virtual IRepositoryConnection<TEntity> UseConnection<TOther>(IRepositoryConnection<TOther> connection)
        where TOther : class
    {
        if (connection is not EntityFrameworkRepositoryConnection<TOther>)
        {
            throw new NotSupportedException("The other connection must be of type EntityFrameworkRepositoryConnection<T>");
        }

        var otherConnection = connection as EntityFrameworkRepositoryConnection<TOther>;
        return new EntityFrameworkRepositoryConnection<TEntity>(otherConnection.Context, false);
    }

    #region Find

    /// <summary>
    ///  Finds all entities in the set.
    /// </summary>
    /// <param name="options"></param>
    /// <returns>A collection of all entities in the set.</returns>
    public virtual IPagedCollection<TEntity> Find(SearchOptions<TEntity> options)
    {
        using var context = GetContext();
        var query = BuildBaseQuery(context, options);
        int totalCount = query.Count();
        query = ApplyPaging(query, options);

        return new PagedList<TEntity>(
            query.ToList(),
            options.PageNumber > 0 ? options.PageNumber - 1 : 1,
            options.PageSize,
            totalCount);
    }

    /// <summary>
    ///  Finds all entities in the set.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="projection"></param>
    /// <returns>A collection of all entities in the set.</returns>
    public virtual IPagedCollection<TResult> Find<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection)
    {
        using var context = GetContext();
        var query = BuildBaseQuery(context, options);
        var projectedQuery = query.Select(projection);

        int totalCount = projectedQuery.Count();
        projectedQuery = ApplyPaging(projectedQuery, options);

        return new PagedList<TResult>(
            projectedQuery.ToList(),
            options.PageNumber > 0 ? options.PageNumber - 1 : 1,
            options.PageSize,
            totalCount);
    }

    /// <summary>
    ///  Finds all entities in the set.
    /// </summary>
    /// <param name="options"></param>
    /// <returns>A collection of all entities in the set.</returns>
    public virtual async Task<IPagedCollection<TEntity>> FindAsync(SearchOptions<TEntity> options)
    {
        using var context = GetContext();
        var query = BuildBaseQuery(context, options);
        int totalCount = await query.CountAsync();
        query = ApplyPaging(query, options);

        return new PagedList<TEntity>(
            await query.ToListAsync(),
            options.PageNumber > 0 ? options.PageNumber - 1 : 1,
            options.PageSize,
            totalCount);
    }

    /// <summary>
    ///  Finds all entities in the set.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="projection"></param>
    /// <returns>A collection of all entities in the set.</returns>
    public virtual async Task<IPagedCollection<TResult>> FindAsync<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection)
    {
        using var context = GetContext();
        var query = BuildBaseQuery(context, options);
        var projectedQuery = query.Select(projection);

        int totalCount = await projectedQuery.CountAsync();
        projectedQuery = ApplyPaging(projectedQuery, options);

        return new PagedList<TResult>(
            await projectedQuery.ToListAsync(),
            options.PageNumber > 0 ? options.PageNumber - 1 : 1,
            options.PageSize,
            totalCount);
    }

    /// <summary>
    ///  Finds an entity with the given primary key values.
    /// </summary>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>The entity found, or null.</returns>
    public virtual TEntity FindOne(params object[] keyValues)
    {
        using var context = GetContext();
        return context.Set<TEntity>().Find(keyValues);
    }

    /// <summary>
    /// Finds an entity based on a predicate.
    /// </summary>
    /// <param name="options"></param>
    /// <returns>The entity found, or null.</returns>
    public virtual TEntity FindOne(SearchOptions<TEntity> options)
    {
        using var context = GetContext();
        var query = BuildBaseQuery(context, options);
        return query.FirstOrDefault();
    }

    /// <summary>
    /// Finds an entity based on a predicate.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="projection"></param>
    /// <returns>The entity found, or null.</returns>
    public virtual TResult FindOne<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection)
    {
        using var context = GetContext();
        var query = BuildBaseQuery(context, options);
        var projectedQuery = query.Select(projection);
        return projectedQuery.FirstOrDefault();
    }

    /// <summary>
    ///  Asynchronously finds an entity with the given primary key values.
    /// </summary>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>The entity found, or null.</returns>
    public virtual async Task<TEntity> FindOneAsync(params object[] keyValues)
    {
        using var context = GetContext();
        return await context.Set<TEntity>().FindAsync(keyValues);
    }

    /// <summary>
    /// Asynchronously finds an entity based on a predicate.
    /// </summary>
    /// <param name="options"></param>
    /// <returns>The entity found, or null.</returns>
    public virtual async Task<TEntity> FindOneAsync(SearchOptions<TEntity> options)
    {
        using var context = GetContext();
        var query = BuildBaseQuery(context, options);
        return await query.FirstOrDefaultAsync();
    }

    /// <summary>
    /// Finds an entity based on a predicate.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="projection"></param>
    /// <returns>The entity found, or null.</returns>
    public virtual async Task<TResult> FindOneAsync<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection)
    {
        using var context = GetContext();
        var query = BuildBaseQuery(context, options);
        var projectedQuery = query.Select(projection);
        return await projectedQuery.FirstOrDefaultAsync();
    }

    #endregion Find

    #region Count

    /// <summary>
    /// Returns the number of elements in a sequence.
    /// </summary>
    /// <returns>The number of elements in the sequence.</returns>
    public virtual int Count()
    {
        using var context = GetContext();
        return context.Set<TEntity>().AsNoTracking().Count();
    }

    /// <summary>
    /// Returns the number of elements in a sequence that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>The number of elements in the sequence that satisfy the condition in the predicate function.</returns>
    public virtual int Count(Expression<Func<TEntity, bool>> predicate)
    {
        using var context = GetContext();
        return context.Set<TEntity>().AsNoTracking().Count(predicate);
    }

    /// <summary>
    /// Asynchronously returns the number of elements in a sequence.
    /// </summary>
    /// <returns>
    /// <para>A task that represents the asynchronous operation. The task result contains the number</para>
    /// <para>of elements in the sequence.</para>
    /// </returns>
    public virtual async Task<int> CountAsync()
    {
        using var context = GetContext();
        return await context.Set<TEntity>().AsNoTracking().CountAsync();
    }

    /// <summary>
    /// Asynchronously returns the number of elements in a sequence that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>
    /// <para>A task that represents the asynchronous operation. The task result contains the number</para>
    /// <para>of elements in the sequence that satisfy the condition in the predicate function.</para>
    /// </returns>
    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        using var context = GetContext();
        return await context.Set<TEntity>().AsNoTracking().CountAsync(predicate);
    }

    /// <summary>
    /// Returns a System.Int64 that represents the number of elements in a sequence.
    /// </summary>
    /// <returns>The number of elements in the sequence.</returns>
    public virtual long LongCount()
    {
        using var context = GetContext();
        return context.Set<TEntity>().AsNoTracking().LongCount();
    }

    /// <summary>
    /// Returns a System.Int64 that represents the number of elements in a sequence that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>The number of elements in the sequence that satisfy the condition in the predicate function.</returns>
    public virtual long LongCount(Expression<Func<TEntity, bool>> predicate)
    {
        using var context = GetContext();
        return context.Set<TEntity>().AsNoTracking().LongCount(predicate);
    }

    /// <summary>
    /// Asynchronously returns a System.Int64 that represents the number of elements in a sequence.
    /// </summary>
    /// <returns>
    /// <para>A task that represents the asynchronous operation. The task result contains the number</para>
    /// <para>of elements in the sequence.</para>
    /// </returns>
    public virtual async Task<long> LongCountAsync()
    {
        using var context = GetContext();
        return await context.Set<TEntity>().AsNoTracking().LongCountAsync();
    }

    /// <summary>
    /// Asynchronously returns a System.Int64 that represents the number of elements in a sequence that satisfy a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>
    /// <para>A task that represents the asynchronous operation. The task result contains the number</para>
    /// <para>of elements in the sequence that satisfy the condition in the predicate function.</para>
    /// </returns>
    public virtual async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        using var context = GetContext();
        return await context.Set<TEntity>().AsNoTracking().LongCountAsync(predicate);
    }

    #endregion Count

    #region Delete

    /// <summary>
    /// Deletes the given entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Delete(TEntity entity)
    {
        using var context = GetContext();
        var set = context.Set<TEntity>();

        if (context.Entry(entity).State == EntityState.Detached)
        {
            var localEntity = set.Local.FirstOrDefault(x => Enumerable.SequenceEqual(x.KeyValues, entity.KeyValues));

            if (localEntity != null)
            {
                context.Entry(localEntity).State = EntityState.Deleted;
            }
            else
            {
                set.Attach(entity);
                context.Entry(entity).State = EntityState.Deleted;
            }
        }
        else
        {
            set.Remove(entity);
        }
        return context.SaveChanges();
    }

    /// <summary>
    /// Deletes the given entities.
    /// </summary>
    /// <param name="entities">The entities to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Delete(IEnumerable<TEntity> entities)
    {
        using var context = GetContext();
        var set = context.Set<TEntity>();

        foreach (var entity in entities)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                var localEntity = set.Local.FirstOrDefault(x => Enumerable.SequenceEqual(x.KeyValues, entity.KeyValues));

                if (localEntity != null)
                {
                    context.Entry(localEntity).State = EntityState.Deleted;
                }
                else
                {
                    set.Attach(entity);
                    context.Entry(entity).State = EntityState.Deleted;
                }
            }
            else
            {
                set.Remove(entity);
            }
        }
        return context.SaveChanges();
    }

    /// <summary>
    /// Deletes all entities that match the given predicate
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
    {
        using var context = GetContext();
        return context.Set<TEntity>().Where(predicate).Delete();
    }

    /// <summary>
    /// Deletes all rows without retrieving entities.
    /// </summary>
    /// <returns>The number of rows affected.</returns>
    public virtual int DeleteAll()
    {
        using var context = GetContext();
        return context.Set<TEntity>().Delete();
    }

    /// <summary>
    /// Asynchronously deletes all rows without retrieving entities.
    /// </summary>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> DeleteAllAsync()
    {
        using var context = GetContext();
        return await context.Set<TEntity>().DeleteAsync();
    }

    /// <summary>
    /// Asynchronously deletes the given entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> DeleteAsync(TEntity entity)
    {
        using var context = GetContext();
        var set = context.Set<TEntity>();

        if (context.Entry(entity).State == EntityState.Detached)
        {
            var localEntity = set.Local.FirstOrDefault(x => Enumerable.SequenceEqual(x.KeyValues, entity.KeyValues));

            if (localEntity != null)
            {
                context.Entry(localEntity).State = EntityState.Deleted;
            }
            else
            {
                set.Attach(entity);
                context.Entry(entity).State = EntityState.Deleted;
            }
        }
        else
        {
            set.Remove(entity);
        }
        return await context.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously deletes the given entities
    /// </summary>
    /// <param name="entities">The entities to delete.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> DeleteAsync(IEnumerable<TEntity> entities)
    {
        using var context = GetContext();
        var set = context.Set<TEntity>();

        foreach (var entity in entities)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                var localEntity = set.Local.FirstOrDefault(x => Enumerable.SequenceEqual(x.KeyValues, entity.KeyValues));

                if (localEntity != null)
                {
                    context.Entry(localEntity).State = EntityState.Deleted;
                }
                else
                {
                    set.Attach(entity);
                    context.Entry(entity).State = EntityState.Deleted;
                }
            }
            else
            {
                set.Remove(entity);
            }
        }
        return await context.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously deletes all entities that match the given predicate
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        using var context = GetContext();
        return await context.Set<TEntity>().Where(predicate).DeleteAsync();
    }

    #endregion Delete

    #region Insert

    /// <summary>
    /// Inserts the given entity.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Insert(TEntity entity)
    {
        using var context = GetContext();
        context.Set<TEntity>().Add(entity);
        return context.SaveChanges();
    }

    /// <summary>
    /// Inserts the given entities.
    /// </summary>
    /// <param name="entities">The entities to insert.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Insert(IEnumerable<TEntity> entities)
    {
        using var context = GetContext();
        context.Set<TEntity>().AddRange(entities);
        return context.SaveChanges();
    }

    /// <summary>
    /// Asynchronously inserts the given entity.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> InsertAsync(TEntity entity)
    {
        using var context = GetContext();
        await context.Set<TEntity>().AddAsync(entity);
        return await context.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously inserts the given entities.
    /// </summary>
    /// <param name="entities">The entities to insert.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> InsertAsync(IEnumerable<TEntity> entities)
    {
        using var context = GetContext();
        await context.Set<TEntity>().AddRangeAsync(entities);
        return await context.SaveChangesAsync();
    }

    #endregion Insert

    #region Update

    /// <summary>
    /// Updates the given entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Update(TEntity entity)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            using var context = GetContext();
            var set = context.Set<TEntity>();

            if (context.Entry(entity).State == EntityState.Detached)
            {
                var localEntity = set.Local.FirstOrDefault(x => Enumerable.SequenceEqual(x.KeyValues, entity.KeyValues));

                if (localEntity != null)
                {
                    context.Entry(localEntity).CurrentValues.SetValues(entity);
                    context.Entry(localEntity).State = EntityState.Modified;
                }
                else
                {
                    entity = set.Attach(entity).Entity;
                    context.Entry(entity).State = EntityState.Modified;
                }
            }
            else
            {
                context.Entry(entity).State = EntityState.Modified;
            }

            return context.SaveChanges();
        }
        catch (Exception x)
        {
            string message = x.GetBaseException().Message;
            logger.LogError(new EventId(), x, message);
            throw new ApplicationException(message);
        }
    }

    /// <summary>
    /// Updates the given entities.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Update(IEnumerable<TEntity> entities)
    {
        try
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            using var context = GetContext();
            var set = context.Set<TEntity>();

            foreach (var entity in entities)
            {
                if (context.Entry(entity).State == EntityState.Detached)
                {
                    var localEntity = set.Local.FirstOrDefault(x => Enumerable.SequenceEqual(x.KeyValues, entity.KeyValues));

                    if (localEntity != null)
                    {
                        context.Entry(localEntity).CurrentValues.SetValues(entity);
                    }
                    else
                    {
                        set.Attach(entity);
                        context.Entry(entity).State = EntityState.Modified;
                    }
                }
                else
                {
                    context.Entry(entity).State = EntityState.Modified;
                }
            }
            return context.SaveChanges();
        }
        catch (Exception x)
        {
            string message = x.GetBaseException().Message;
            logger.LogError(new EventId(), x, message);
            throw new ApplicationException(message);
        }
    }

    /// <summary>
    /// Updates all rows using an expression without retrieving entities.
    /// </summary>
    /// <param name="updateFactory">The update expression.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Update(Expression<Func<TEntity, TEntity>> updateFactory)
    {
        using var context = GetContext();
        return context.Set<TEntity>().Update(updateFactory);
    }

    /// <summary>
    /// Updates all rows that match the given predicate using an expression without retrieving entities.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="updateFactory">The update expression.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateFactory)
    {
        using var context = GetContext();
        return context.Set<TEntity>().Where(predicate).Update(updateFactory);
    }

    /// <summary>
    /// Updates all rows from the query using an expression without retrieving entities.
    /// </summary>
    /// <param name="query">The query to update rows from without retrieving entities.</param>
    /// <param name="updateFactory">The update expression.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Update(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateFactory) =>
        query.Update(updateFactory);

    /// <summary>
    /// Asynchronously updates the given entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> UpdateAsync(TEntity entity)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            using var context = GetContext();
            var set = context.Set<TEntity>();

            if (context.Entry(entity).State == EntityState.Detached)
            {
                var localEntity = set.Local.FirstOrDefault(x => Enumerable.SequenceEqual(x.KeyValues, entity.KeyValues));

                if (localEntity != null)
                {
                    context.Entry(localEntity).CurrentValues.SetValues(entity);
                    context.Entry(localEntity).State = EntityState.Modified;
                }
                else
                {
                    entity = set.Attach(entity).Entity;
                    context.Entry(entity).State = EntityState.Modified;
                }
            }
            else
            {
                context.Entry(entity).State = EntityState.Modified;
            }

            return await context.SaveChangesAsync();
        }
        catch (Exception x)
        {
            string message = x.GetBaseException().Message;
            logger.LogError(new EventId(), x, message);
            throw new ApplicationException(message);
        }
    }

    /// <summary>
    /// Asynchronously updates the given entities.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> UpdateAsync(IEnumerable<TEntity> entities)
    {
        try
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            using var context = GetContext();
            var set = context.Set<TEntity>();

            foreach (var entity in entities)
            {
                if (context.Entry(entity).State == EntityState.Detached)
                {
                    var localEntity = set.Local.FirstOrDefault(x => Enumerable.SequenceEqual(x.KeyValues, entity.KeyValues));

                    if (localEntity != null)
                    {
                        context.Entry(localEntity).CurrentValues.SetValues(entity);
                    }
                    else
                    {
                        set.Attach(entity);
                        context.Entry(entity).State = EntityState.Modified;
                    }
                }
                else
                {
                    context.Entry(entity).State = EntityState.Modified;
                }
            }
            return await context.SaveChangesAsync();
        }
        catch (Exception x)
        {
            string message = x.GetBaseException().Message;
            logger.LogError(new EventId(), x, message);
            throw new ApplicationException(message);
        }
    }

    /// <summary>
    /// Asynchronously updates all rows using an expression without retrieving entities.
    /// </summary>
    /// <param name="updateFactory">The update expression.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, TEntity>> updateFactory)
    {
        using var context = GetContext();
        return await context.Set<TEntity>().UpdateAsync(updateFactory);
    }

    /// <summary>
    /// Asynchronously updates all rows that match the given predicate using an expression without retrieving entities.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="updateFactory">The update expression.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateFactory)
    {
        using var context = GetContext();
        return await context.Set<TEntity>().Where(predicate).UpdateAsync(updateFactory);
    }

    /// <summary>
    /// Asynchronously updates all rows from the query using an expression without retrieving entities.
    /// </summary>
    /// <param name="query">The query to update rows from without retrieving entities.</param>
    /// <param name="updateFactory">The update expression.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> UpdateAsync(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateFactory) =>
        await query.UpdateAsync(updateFactory);

    #endregion Update

    #endregion IRepository<TEntity> Members

    /// <summary>
    /// Returns an instance of the DbContext used in this EntityFrameworkRepository&lt;TEntity&gt;
    /// </summary>
    /// <returns>An instance of the DbContext.</returns>
    protected virtual DbContext GetContext() => contextFactory.GetContext();

    private static IQueryable<TEntity> BuildBaseQuery(DbContext context, SearchOptions<TEntity> options)
    {
        var query = context.Set<TEntity>().AsNoTracking();

        if (options.Include is not null)
        {
            query = options.Include.Compile()(query);
        }

        if (options.Query is not null)
        {
            query = query.Where(options.Query);
        }

        if (options.OrderBy is not null)
        {
            query = options.OrderBy.Compile()(query);
        }

        return query;
    }

    private static IQueryable<T> ApplyPaging<T>(IQueryable<T> query, SearchOptions<TEntity> options)
    {
        if (options.PageSize > 0 && options.PageNumber > 0)
        {
            query = query
                .Skip((options.PageNumber - 1) * options.PageSize)
                .Take(options.PageSize);
        }
        return query;
    }
}
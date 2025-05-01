using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Extenso.Data.Entity;
using Extenso.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Z.EntityFramework.Plus;

namespace Extenso.Data.Entity;

public abstract class MappedEntityFrameworkRepository<TModel, TEntity> : IMappedRepository<TModel, TEntity>
    where TModel : class
    where TEntity : class, IEntity
{
    #region Non-Public Members

    private readonly ILogger logger;

    protected IDbContextFactory contextFactory;

    #endregion Non-Public Members

    #region Constructor

    public MappedEntityFrameworkRepository(
        IDbContextFactory contextFactory,
        ILoggerFactory loggerFactory)
    {
        this.contextFactory = contextFactory;
        logger = loggerFactory.CreateLogger<MappedEntityFrameworkRepository<TModel, TEntity>>();
    }

    #endregion Constructor

    #region IRepository<TModel> Members

    /// <summary>
    /// <para>This is typically used to create queries with custom projections. For example, you may wish</para>
    /// <para>to select only a small number of columns from the table.</para>
    /// </summary>
    /// <returns>An instance of IRepositoryConnectionlt;TModel&gt;</returns>
    public virtual IRepositoryConnection<TModel> OpenConnection()
    {
        var context = contextFactory.GetContext();
        return new EntityFrameworkRepositoryConnection<TModel>(context, true);
    }

    /// <summary>
    /// <para>If OpenConnection() has already been called and you wish to use the same connection</para>
    /// <para> (same DbContext), then use this.</para>
    /// </summary>
    /// <typeparam name="TOther">The type used in the other connection.</typeparam>
    /// <param name="connection">An instance of IRepositoryConnectionlt;TOther&gt;</param>
    /// <returns>An instance of IRepositoryConnectionlt;TModel&gt;</returns>
    public virtual IRepositoryConnection<TModel> UseConnection<TOther>(IRepositoryConnection<TOther> connection)
        where TOther : class
    {
        if (connection is not EntityFrameworkRepositoryConnection<TOther>)
        {
            throw new NotSupportedException("The other connection must be of type EntityFrameworkRepositoryConnection<T>");
        }

        var otherConnection = connection as EntityFrameworkRepositoryConnection<TOther>;
        return new EntityFrameworkRepositoryConnection<TModel>(otherConnection.Context, false);
    }

    #region Find

    /// <summary>
    ///  Finds all entities in the set.
    /// </summary>
    /// <param name="includePaths">Specifies related entities to include in the query results.</param>
    /// <returns>A collection of all entities in the set.</returns>
    public virtual IEnumerable<TModel> Find(params Expression<Func<TModel, object>>[] includePaths)
    {
        using var context = GetContext();
        var query = context.Set<TEntity>().AsNoTracking();

        var mappedIncludePaths = includePaths.Select(x => MapIncludeExpression(x)).ToArray();
        foreach (var path in mappedIncludePaths)
        {
            query = query.Include(path);
        }

        var entities = query.ToList();
        return entities.Select(x => ToModel(x)).ToList();
    }

    /// <summary>
    /// Finds a filtered list of entities based on a predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="includePaths">Specifies related entities to include in the query results.</param>
    /// <returns>A filtered list of entities based on a predicate.</returns>
    public virtual IEnumerable<TModel> Find(Expression<Func<TModel, bool>> predicate, params Expression<Func<TModel, object>>[] includePaths)
    {
        using var context = GetContext();
        var query = context.Set<TEntity>().AsNoTracking();

        var mappedIncludePaths = includePaths.Select(x => MapIncludeExpression(x)).ToArray();
        foreach (var path in mappedIncludePaths)
        {
            query = query.Include(path);
        }

        var mappedPredicate = MapPredicateExpression(predicate);
        var entities = query.Where(mappedPredicate).ToList();
        return entities.Select(x => ToModel(x)).ToList();
    }

    /// <summary>
    /// Asynchronously finds all entities in the set.
    /// </summary>
    /// <param name="includePaths">Specifies related entities to include in the query results.</param>
    /// <returns>A collection of all entities in the set.</returns>
    public virtual async Task<IEnumerable<TModel>> FindAsync(params Expression<Func<TModel, object>>[] includePaths)
    {
        using var context = GetContext();
        var query = context.Set<TEntity>().AsNoTracking();

        var mappedIncludePaths = includePaths.Select(x => MapIncludeExpression(x)).ToArray();
        foreach (var path in mappedIncludePaths)
        {
            query = query.Include(path);
        }

        var entities = await query.ToListAsync();
        return entities.Select(x => ToModel(x)).ToList();
    }

    /// <summary>
    /// Asynchronously finds a filtered list of entities based on a predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="includePaths">Specifies related entities to include in the query results.</param>
    /// <returns>A filtered list of entities based on a predicate.</returns>
    public virtual async Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, params Expression<Func<TModel, object>>[] includePaths)
    {
        using var context = GetContext();
        var query = context.Set<TEntity>().AsNoTracking();

        var mappedIncludePaths = includePaths.Select(x => MapIncludeExpression(x)).ToArray();
        foreach (var path in mappedIncludePaths)
        {
            query = query.Include(path);
        }

        var mappedPredicate = MapPredicateExpression(predicate);
        var entities = await query.Where(mappedPredicate).ToListAsync();
        return entities.Select(x => ToModel(x)).ToList();
    }

    /// <summary>
    ///  Finds an entity with the given primary key values.
    /// </summary>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>The entity found, or null.</returns>
    public virtual TModel FindOne(params object[] keyValues)
    {
        using var context = GetContext();
        var entity = context.Set<TEntity>().Find(keyValues);
        return ToModel(entity);
    }

    /// <summary>
    /// Finds an entity based on a predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="includePaths">Specifies related entities to include in the query results.</param>
    /// <returns>The entity found, or null.</returns>
    public virtual TModel FindOne(Expression<Func<TModel, bool>> predicate, params Expression<Func<TModel, object>>[] includePaths)
    {
        var mappedPredicate = MapPredicateExpression(predicate);

        using var context = GetContext();
        var query = context.Set<TEntity>().AsNoTracking().Where(mappedPredicate);

        var mappedIncludePaths = includePaths.Select(x => MapIncludeExpression(x)).ToArray();
        foreach (var path in mappedIncludePaths)
        {
            query = query.Include(path);
        }

        var entity = query.FirstOrDefault();
        return ToModel(entity);
    }

    /// <summary>
    ///  Asynchronously finds an entity with the given primary key values.
    /// </summary>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>The entity found, or null.</returns>
    public virtual async Task<TModel> FindOneAsync(params object[] keyValues)
    {
        using var context = GetContext();
        var entity = await context.Set<TEntity>().FindAsync(keyValues);
        return ToModel(entity);
    }

    /// <summary>
    /// Asynchronously finds an entity based on a predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="includePaths">Specifies related entities to include in the query results.</param>
    /// <returns>The entity found, or null.</returns>
    public virtual async Task<TModel> FindOneAsync(Expression<Func<TModel, bool>> predicate, params Expression<Func<TModel, object>>[] includePaths)
    {
        var mappedPredicate = MapPredicateExpression(predicate);

        using var context = GetContext();
        var query = context.Set<TEntity>().AsNoTracking().Where(mappedPredicate);

        var mappedIncludePaths = includePaths.Select(x => MapIncludeExpression(x)).ToArray();
        foreach (var path in mappedIncludePaths)
        {
            query = query.Include(path);
        }

        var entity = await query.FirstOrDefaultAsync();
        return ToModel(entity);
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
    public virtual int Count(Expression<Func<TModel, bool>> predicate)
    {
        var mappedPredicate = MapPredicateExpression(predicate);
        using var context = GetContext();
        return context.Set<TEntity>().AsNoTracking().Count(mappedPredicate);
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
    public virtual async Task<int> CountAsync(Expression<Func<TModel, bool>> predicate)
    {
        var mappedPredicate = MapPredicateExpression(predicate);
        using var context = GetContext();
        return await context.Set<TEntity>().AsNoTracking().CountAsync(mappedPredicate);
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
    public virtual long LongCount(Expression<Func<TModel, bool>> predicate)
    {
        var mappedPredicate = MapPredicateExpression(predicate);
        using var context = GetContext();
        return context.Set<TEntity>().AsNoTracking().LongCount(mappedPredicate);
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
    public virtual async Task<long> LongCountAsync(Expression<Func<TModel, bool>> predicate)
    {
        var mappedPredicate = MapPredicateExpression(predicate);
        using var context = GetContext();
        return await context.Set<TEntity>().AsNoTracking().LongCountAsync(mappedPredicate);
    }

    #endregion Count

    #region Delete

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
    /// Deletes the given entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Delete(TModel model)
    {
        var entity = ToEntity(model);

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
    public virtual int Delete(IEnumerable<TModel> models)
    {
        var entities = models.Select(x => ToEntity(x)).ToList();

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
    public virtual int Delete(Expression<Func<TModel, bool>> predicate)
    {
        var mappedPredicate = MapPredicateExpression(predicate);
        using var context = GetContext();
        return context.Set<TEntity>().Where(mappedPredicate).Delete();
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
    public virtual async Task<int> DeleteAsync(TModel model)
    {
        var entity = ToEntity(model);

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
    public virtual async Task<int> DeleteAsync(IEnumerable<TModel> models)
    {
        var entities = models.Select(x => ToEntity(x)).ToList();

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
    public virtual async Task<int> DeleteAsync(Expression<Func<TModel, bool>> predicate)
    {
        var mappedPredicate = MapPredicateExpression(predicate);
        using var context = GetContext();
        return await context.Set<TEntity>().Where(mappedPredicate).DeleteAsync();
    }

    #endregion Delete

    #region Insert

    /// <summary>
    /// Inserts the given entity.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Insert(TModel model)
    {
        var entity = ToEntity(model);
        using var context = GetContext();
        context.Set<TEntity>().Add(entity);
        return context.SaveChanges();
    }

    /// <summary>
    /// Inserts the given entities.
    /// </summary>
    /// <param name="entities">The entities to insert.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Insert(IEnumerable<TModel> models)
    {
        var entities = models.Select(x => ToEntity(x)).ToList();
        using var context = GetContext();
        context.Set<TEntity>().AddRange(entities);
        return context.SaveChanges();
    }

    /// <summary>
    /// Asynchronously inserts the given entity.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> InsertAsync(TModel model)
    {
        var entity = ToEntity(model);
        using var context = GetContext();
        await context.Set<TEntity>().AddAsync(entity);
        return await context.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously inserts the given entities.
    /// </summary>
    /// <param name="entities">The entities to insert.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> InsertAsync(IEnumerable<TModel> models)
    {
        var entities = models.Select(x => ToEntity(x)).ToList();
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
    public virtual int Update(TModel model)
    {
        try
        {
            var entity = ToEntity(model);

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
    public virtual int Update(IEnumerable<TModel> models)
    {
        try
        {
            var entities = models.Select(x => ToEntity(x)).ToList();

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
    /// Asynchronously updates the given entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> UpdateAsync(TModel model)
    {
        try
        {
            var entity = ToEntity(model);

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
    public virtual async Task<int> UpdateAsync(IEnumerable<TModel> models)
    {
        try
        {
            var entities = models.Select(x => ToEntity(x)).ToList();

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
    /// Updates all rows using an expression without retrieving entities.
    /// </summary>
    /// <param name="updateFactory">The update expression.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Update(Expression<Func<TModel, TModel>> updateFactory)
    {
        var mappedUpdateExpression = MapUpdateExpression(updateFactory);
        using var context = GetContext();
        return context.Set<TEntity>().Update(mappedUpdateExpression);
    }

    /// <summary>
    /// Updates all rows that match the given predicate using an expression without retrieving entities.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="updateFactory">The update expression.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Update(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TModel>> updateFactory)
    {
        var mappedPredicate = MapPredicateExpression(predicate);
        var mappedUpdateExpression = MapUpdateExpression(updateFactory);
        using var context = GetContext();
        return context.Set<TEntity>().Where(mappedPredicate).Update(mappedUpdateExpression);
    }

    /// <summary>
    /// Updates all rows from the query using an expression without retrieving entities.
    /// </summary>
    /// <param name="query">The query to update rows from without retrieving entities.</param>
    /// <param name="updateFactory">The update expression.</param>
    /// <returns>The number of rows affected.</returns>
    public virtual int Update(IQueryable<TModel> query, Expression<Func<TModel, TModel>> updateFactory) =>
        throw new NotSupportedException();

    /// <summary>
    /// Asynchronously updates all rows using an expression without retrieving entities.
    /// </summary>
    /// <param name="updateFactory">The update expression.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> UpdateAsync(Expression<Func<TModel, TModel>> updateFactory)
    {
        var mappedUpdateExpression = MapUpdateExpression(updateFactory);
        using var context = GetContext();
        return await context.Set<TEntity>().UpdateAsync(mappedUpdateExpression);
    }

    /// <summary>
    /// Asynchronously updates all rows that match the given predicate using an expression without retrieving entities.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="updateFactory">The update expression.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> UpdateAsync(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TModel>> updateFactory)
    {
        var mappedPredicate = MapPredicateExpression(predicate);
        var mappedUpdateExpression = MapUpdateExpression(updateFactory);
        using var context = GetContext();
        return await context.Set<TEntity>().Where(mappedPredicate).UpdateAsync(mappedUpdateExpression);
    }

    /// <summary>
    /// Asynchronously updates all rows from the query using an expression without retrieving entities.
    /// </summary>
    /// <param name="query">The query to update rows from without retrieving entities.</param>
    /// <param name="updateFactory">The update expression.</param>
    /// <returns>A task with the number of rows affected.</returns>
    public virtual async Task<int> UpdateAsync(IQueryable<TModel> query, Expression<Func<TModel, TModel>> updateFactory) =>
        throw new NotSupportedException();

    #endregion Update

    #endregion IRepository<TModel> Members

    /// <summary>
    /// Returns an instance of the DbContext used in this EntityFrameworkRepository&lt;TModel&gt;
    /// </summary>
    /// <returns>An instance of the DbContext.</returns>
    protected virtual DbContext GetContext() => contextFactory.GetContext();

    public abstract TModel ToModel(TEntity entity);

    public abstract TEntity ToEntity(TModel model);

    public abstract Expression<Func<TEntity, bool>> MapPredicateExpression(Expression<Func<TModel, bool>> predicate);

    public abstract Expression<Func<TEntity, TEntity>> MapUpdateExpression(Expression<Func<TModel, TModel>> updateExpression);

    public abstract Expression<Func<TEntity, TProperty>> MapIncludeExpression<TProperty>(Expression<Func<TModel, TProperty>> includeExpression);
}
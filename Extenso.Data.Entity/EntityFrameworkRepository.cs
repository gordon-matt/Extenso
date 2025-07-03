﻿using System.Linq.Expressions;
using Extenso.Collections;
using Extenso.Collections.Generic;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Z.EntityFramework.Plus;

namespace Extenso.Data.Entity;

public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>, IEntityFrameworkRepository
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

    /// <inheritdoc/>
    public virtual IRepositoryConnection<TEntity> OpenConnection(ContextOptions options = null)
    {
        var context = GetContext(options);
        return new EntityFrameworkRepositoryConnection<TEntity>(context, true);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public virtual IPagedCollection<TEntity> Find(SearchOptions<TEntity> options)
    {
        using var context = GetContext(options);
        var query = BuildBaseQuery(context, options);
        int totalCount = query.Count();
        query = ApplyPaging(query, options);

        return new PagedList<TEntity>(
            query.ToList(),
            options.PageNumber > 0 ? options.PageNumber - 1 : 1,
            options.PageSize,
            totalCount);
    }

    /// <inheritdoc/>
    public virtual IPagedCollection<TResult> Find<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection)
    {
        using var context = GetContext(options);
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

    /// <inheritdoc/>
    public virtual async Task<IPagedCollection<TEntity>> FindAsync(SearchOptions<TEntity> options)
    {
        using var context = GetContext(options);
        var query = BuildBaseQuery(context, options);
        int totalCount = await query.CountAsync();
        query = ApplyPaging(query, options);

        return new PagedList<TEntity>(
            await query.ToListAsync(),
            options.PageNumber > 0 ? options.PageNumber - 1 : 1,
            options.PageSize,
            totalCount);
    }

    /// <inheritdoc/>
    public virtual async Task<IPagedCollection<TResult>> FindAsync<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection)
    {
        using var context = GetContext(options);
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

    /// <inheritdoc/>
    public virtual TEntity FindOne(params object[] keyValues)
    {
        using var context = GetContext();
        return context.Set<TEntity>().Find(keyValues);
    }

    /// <inheritdoc/>
    public virtual TEntity FindOne(SearchOptions<TEntity> options)
    {
        using var context = GetContext(options);
        var query = BuildBaseQuery(context, options);
        return query.FirstOrDefault();
    }

    /// <inheritdoc/>
    public virtual TResult FindOne<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection)
    {
        using var context = GetContext(options);
        var query = BuildBaseQuery(context, options);
        var projectedQuery = query.Select(projection);
        return projectedQuery.FirstOrDefault();
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity> FindOneAsync(params object[] keyValues)
    {
        using var context = GetContext();
        return await context.Set<TEntity>().FindAsync(keyValues);
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity> FindOneAsync(SearchOptions<TEntity> options)
    {
        using var context = GetContext(options);
        var query = BuildBaseQuery(context, options);
        return await query.FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<TResult> FindOneAsync<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection)
    {
        using var context = GetContext(options);
        var query = BuildBaseQuery(context, options);
        var projectedQuery = query.Select(projection);
        return await projectedQuery.FirstOrDefaultAsync();
    }

    #endregion Find

    #region Count

    /// <inheritdoc/>
    public virtual int Count(ContextOptions options = null)
    {
        using var context = GetContext(options);
        return context.Set<TEntity>().AsNoTracking().Count();
    }

    /// <inheritdoc/>
    public virtual int Count(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return context.Set<TEntity>().AsNoTracking().Count(predicate);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().AsNoTracking().CountAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().AsNoTracking().CountAsync(predicate);
    }

    /// <inheritdoc/>
    public virtual long LongCount(ContextOptions options = null)
    {
        using var context = GetContext(options);
        return context.Set<TEntity>().AsNoTracking().LongCount();
    }

    /// <inheritdoc/>
    public virtual long LongCount(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return context.Set<TEntity>().AsNoTracking().LongCount(predicate);
    }

    /// <inheritdoc/>
    public virtual async Task<long> LongCountAsync(ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().AsNoTracking().LongCountAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().AsNoTracking().LongCountAsync(predicate);
    }

    #endregion Count

    #region Delete

    /// <inheritdoc/>
    public virtual int Delete(TEntity entity, ContextOptions options = null)
    {
        using var context = GetContext(options);
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

    /// <inheritdoc/>
    public virtual int Delete(IEnumerable<TEntity> entities, ContextOptions options = null)
    {
        using var context = GetContext(options);
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

    /// <inheritdoc/>
    public virtual int Delete(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return context.Set<TEntity>().Where(predicate).ExecuteDelete();
    }

    /// <inheritdoc/>
    public virtual int DeleteAll(ContextOptions options = null)
    {
        using var context = GetContext(options);
        return context.Set<TEntity>().Delete();
    }

    /// <inheritdoc/>
    public virtual async Task<int> DeleteAllAsync(ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().DeleteAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<int> DeleteAsync(TEntity entity, ContextOptions options = null)
    {
        using var context = GetContext(options);
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

    /// <inheritdoc/>
    public virtual async Task<int> DeleteAsync(IEnumerable<TEntity> entities, ContextOptions options = null)
    {
        using var context = GetContext(options);
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

    /// <inheritdoc/>
    public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().Where(predicate).ExecuteDeleteAsync();
    }

    #endregion Delete

    #region Insert

    /// <inheritdoc/>
    public virtual TEntity Insert(TEntity entity, ContextOptions options = null)
    {
        using var context = GetContext(options);
        context.Set<TEntity>().Add(entity);
        context.SaveChanges();
        return entity;
    }

    /// <inheritdoc/>
    public virtual IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities, ContextOptions options = null)
    {
        using var context = GetContext(options);
        context.Set<TEntity>().AddRange(entities);
        context.SaveChanges();
        return entities;
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity> InsertAsync(TEntity entity, ContextOptions options = null)
    {
        using var context = GetContext(options);
        await context.Set<TEntity>().AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities, ContextOptions options = null)
    {
        using var context = GetContext(options);
        await context.Set<TEntity>().AddRangeAsync(entities);
        await context.SaveChangesAsync();
        return entities;
    }

    #endregion Insert

    #region Update

    /// <inheritdoc/>
    public virtual TEntity Update(TEntity entity, ContextOptions options = null)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(entity);

            using var context = GetContext(options);
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

            context.SaveChanges();
            return entity;
        }
        catch (Exception x)
        {
            string message = x.GetBaseException().Message;
            logger.LogError(new EventId(), x, message);
            throw new ApplicationException(message);
        }
    }

    /// <inheritdoc/>
    public virtual IEnumerable<TEntity> Update(IEnumerable<TEntity> entities, ContextOptions options = null)
    {
        try
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            using var context = GetContext(options);
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

            context.SaveChanges();
            return entities;
        }
        catch (Exception x)
        {
            string message = x.GetBaseException().Message;
            logger.LogError(new EventId(), x, message);
            throw new ApplicationException(message);
        }
    }

    /// <inheritdoc/>
    public virtual int Update(Expression<Func<TEntity, TEntity>> updateFactory, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return context.Set<TEntity>().Update(updateFactory);
    }

    /// <inheritdoc/>
    public virtual int Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateFactory, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return context.Set<TEntity>().Where(predicate).Update(updateFactory);
    }

    /// <inheritdoc/>
    public virtual int Update(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateFactory, ContextOptions options = null) =>
        query.Update(updateFactory);

    /// <inheritdoc/>
    public virtual async Task<TEntity> UpdateAsync(TEntity entity, ContextOptions options = null)
    {
        try
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            using var context = GetContext(options);
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

            await context.SaveChangesAsync();
            return entity;
        }
        catch (Exception x)
        {
            string message = x.GetBaseException().Message;
            logger.LogError(new EventId(), x, message);
            throw new ApplicationException(message);
        }
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities, ContextOptions options = null)
    {
        try
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            using var context = GetContext(options);
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

            await context.SaveChangesAsync();
            return entities;
        }
        catch (Exception x)
        {
            string message = x.GetBaseException().Message;
            logger.LogError(new EventId(), x, message);
            throw new ApplicationException(message);
        }
    }

    /// <inheritdoc/>
    public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, TEntity>> updateFactory, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().UpdateAsync(updateFactory);
    }

    /// <inheritdoc/>
    public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateFactory, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().Where(predicate).UpdateAsync(updateFactory);
    }

    /// <inheritdoc/>
    public virtual async Task<int> UpdateAsync(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateFactory) =>
        await query.UpdateAsync(updateFactory);

    #endregion Update

    #endregion IRepository<TEntity> Members

    #region  IEntityFrameworkRepository<TEntity> Members

    /// <inheritdoc/>
    public DbContext GetContext(ContextOptions options = null)
    {
        var context = contextFactory.GetContext();

        if (options is not null)
        {
            if (options.CommandTimeout.HasValue)
            {
                context.Database.SetCommandTimeout(options.CommandTimeout.Value);
            }

            if (options.Transaction is not null)
            {
                context.Database.UseTransaction(options.Transaction);
            }
        }

        return context;
    }

    #endregion

    private IQueryable<TEntity> BuildBaseQuery(DbContext context, SearchOptions<TEntity> options)
    {
        var query = context.Set<TEntity>().AsNoTracking();

        if (options.TagWithCallSite)
        {
            query = query.TagWith(options.CallSiteTag);
        }

        if (!options.Tags.IsNullOrEmpty())
        {
            foreach (string tag in options.Tags)
            {
                query = query.TagWith(tag);
            }
        }

        if (options.Include is not null)
        {
            query = options.Include.Compile()(query);

            if (options.SplitQuery)
            {
                query = query.AsSplitQuery();
            }
        }

        var predicate = PredicateBuilder.New<TEntity>(true);

        if (options.Query is not null)
        {
            predicate = predicate.And(options.Query);
        }

        if (!options.IgnoreMandatoryFilters)
        {
            predicate = ApplyMandatoryFilters(predicate, options.MandatoryFilters);
        }

        query = query.Where(predicate);

        if (options.OrderBy is not null)
        {
            query = options.OrderBy.Compile()(query);
        }

        return query;
    }

    protected virtual Expression<Func<TEntity, bool>> ApplyMandatoryFilters(Expression<Func<TEntity, bool>> predicate, IDictionary<string, object> filters) => predicate;

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
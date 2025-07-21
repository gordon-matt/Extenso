using System.Linq.Expressions;
using Extenso.Collections;
using Extenso.Collections.Generic;
using Extenso.Reflection;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Extenso.Data.Entity;

public class MappedEntityFrameworkRepository<TModel, TEntity> : IMappedRepository<TModel, TEntity>, IEntityFrameworkRepository
    where TModel : class
    where TEntity : class, IEntity
{
    #region Non-Public Members

    protected IDbContextFactory contextFactory;
    private readonly IEntityModelMapper<TEntity, TModel> mapper;

    #endregion Non-Public Members

    #region Constructor

    public MappedEntityFrameworkRepository(
        IDbContextFactory contextFactory,
        IEntityModelMapper<TEntity, TModel> mapper)
    {
        this.contextFactory = contextFactory;
        this.mapper = mapper;
    }

    #endregion Constructor

    #region IRepository<TModel> Members

    /// <inheritdoc/>
    public virtual IRepositoryConnection<TModel> OpenConnection(ContextOptions options = null)
    {
#pragma warning disable DF0010 // Should not be disposed here. Call Dispose() on the IRepositoryConnection instead.
        var context = GetContext(options);
#pragma warning restore DF0010

        return new MappedEntityFrameworkRepositoryConnection<TEntity, TModel>(context, mapper, true);
    }

    /// <inheritdoc/>
    public virtual IRepositoryConnection<TModel> UseConnection<TOther>(IRepositoryConnection<TOther> connection)
        where TOther : class
    {
        if (!connection.GetType().IsAssignableToGenericType(typeof(MappedEntityFrameworkRepositoryConnection<,>)))
        {
            throw new NotSupportedException("The other connection must be of type MappedEntityFrameworkRepositoryConnection<,>");
        }

        var otherConnection = connection as IEntityFrameworkRepositoryConnection<TOther>;
        return new MappedEntityFrameworkRepositoryConnection<TEntity, TModel>(otherConnection.Context, mapper, false);
    }

    #region Find

    /// <inheritdoc/>
    public IPagedCollection<TModel> Find(SearchOptions<TEntity> options)
    {
        using var context = GetContext(options);
        var query = BuildBaseQuery(context, options);
        int totalCount = query.Count();
        query = ApplyPaging(query, options);

        return new PagedList<TModel>(
            query.ToList().Select(mapper.ToModel).ToList(),
            options.PageNumber > 0 ? options.PageNumber - 1 : 1,
            options.PageSize,
            totalCount);
    }

    /// <inheritdoc/>
    public IPagedCollection<TResult> Find<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection)
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
    public async Task<IPagedCollection<TModel>> FindAsync(SearchOptions<TEntity> options)
    {
        var cancellationToken = options?.CancellationToken ?? default;

        using var context = GetContext(options);
        var query = BuildBaseQuery(context, options);
        int totalCount = await query.CountAsync(cancellationToken);
        query = ApplyPaging(query, options);

        return new PagedList<TModel>(
            (await query.ToListAsync(cancellationToken)).Select(mapper.ToModel).ToList(),
            options.PageNumber > 0 ? options.PageNumber - 1 : 1,
            options.PageSize,
            totalCount);
    }

    /// <inheritdoc/>
    public async Task<IPagedCollection<TResult>> FindAsync<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection)
    {
        var cancellationToken = options?.CancellationToken ?? default;

        using var context = GetContext(options);
        var query = BuildBaseQuery(context, options);
        var projectedQuery = query.Select(projection);

        int totalCount = await projectedQuery.CountAsync(cancellationToken);
        projectedQuery = ApplyPaging(projectedQuery, options);

        return new PagedList<TResult>(
            await projectedQuery.ToListAsync(cancellationToken),
            options.PageNumber > 0 ? options.PageNumber - 1 : 1,
            options.PageSize,
            totalCount);
    }

    /// <inheritdoc/>
    public virtual TModel FindOne(params object[] keyValues)
    {
        using var context = GetContext();
        var entity = context.Set<TEntity>().Find(keyValues);
        return mapper.ToModel(entity);
    }

    /// <inheritdoc/>
    public TModel FindOne(SearchOptions<TEntity> options)
    {
        using var context = GetContext(options);
        var query = BuildBaseQuery(context, options);
        var entity = query.FirstOrDefault();
        return mapper.ToModel(entity);
    }

    /// <inheritdoc/>
    public TResult FindOne<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection)
    {
        using var context = GetContext(options);
        var query = BuildBaseQuery(context, options);
        var projectedQuery = query.Select(projection);
        return projectedQuery.FirstOrDefault();
    }

    /// <inheritdoc/>
    public virtual async Task<TModel> FindOneAsync(params object[] keyValues)
    {
        using var context = GetContext();
        var entity = await context.Set<TEntity>().FindAsync(keyValues);
        return mapper.ToModel(entity);
    }

    /// <inheritdoc/>
    public async Task<TModel> FindOneAsync(SearchOptions<TEntity> options)
    {
        using var context = GetContext(options);
        var query = BuildBaseQuery(context, options);
        var entity = await query.FirstOrDefaultAsync(options?.CancellationToken ?? default);
        return mapper.ToModel(entity);
    }

    /// <inheritdoc/>
    public async Task<TResult> FindOneAsync<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection)
    {
        using var context = GetContext(options);
        var query = BuildBaseQuery(context, options);
        var projectedQuery = query.Select(projection);
        return await projectedQuery.FirstOrDefaultAsync(options?.CancellationToken ?? default);
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
    public virtual int Count(Expression<Func<TModel, bool>> predicate, ContextOptions options = null)
    {
        var mappedPredicate = mapper.MapPredicate(predicate);
        using var context = GetContext(options);
        return context.Set<TEntity>().AsNoTracking().Count(mappedPredicate);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().AsNoTracking().CountAsync(options?.CancellationToken ?? default);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(Expression<Func<TModel, bool>> predicate, ContextOptions options = null)
    {
        var mappedPredicate = mapper.MapPredicate(predicate);
        using var context = GetContext(options);
        return await context.Set<TEntity>().AsNoTracking().CountAsync(mappedPredicate, options?.CancellationToken ?? default);
    }

    /// <inheritdoc/>
    public virtual long LongCount(ContextOptions options = null)
    {
        using var context = GetContext(options);
        return context.Set<TEntity>().AsNoTracking().LongCount();
    }

    /// <inheritdoc/>
    public virtual long LongCount(Expression<Func<TModel, bool>> predicate, ContextOptions options = null)
    {
        var mappedPredicate = mapper.MapPredicate(predicate);
        using var context = GetContext(options);
        return context.Set<TEntity>().AsNoTracking().LongCount(mappedPredicate);
    }

    /// <inheritdoc/>
    public virtual async Task<long> LongCountAsync(ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().AsNoTracking().LongCountAsync(options?.CancellationToken ?? default);
    }

    /// <inheritdoc/>
    public virtual async Task<long> LongCountAsync(Expression<Func<TModel, bool>> predicate, ContextOptions options = null)
    {
        var mappedPredicate = mapper.MapPredicate(predicate);
        using var context = GetContext(options);
        return await context.Set<TEntity>().AsNoTracking().LongCountAsync(mappedPredicate, options?.CancellationToken ?? default);
    }

    #endregion Count

    #region Delete

    /// <inheritdoc/>
    public virtual int Delete(TModel model, ContextOptions options = null)
    {
        var entity = mapper.ToEntity(model);

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
    public virtual int Delete(IEnumerable<TModel> models, ContextOptions options = null)
    {
        var entities = models.Select(mapper.ToEntity).ToList();

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
    public virtual int Delete(Expression<Func<TModel, bool>> predicate, ContextOptions options = null)
    {
        var mappedPredicate = mapper.MapPredicate(predicate);
        using var context = GetContext(options);
        return context.Set<TEntity>().Where(mappedPredicate).ExecuteDelete();
    }

    /// <inheritdoc/>
    public virtual int DeleteAll(ContextOptions options = null)
    {
        using var context = GetContext(options);
        return context.Set<TEntity>().ExecuteDelete();
    }

    /// <inheritdoc/>
    public virtual async Task<int> DeleteAllAsync(ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().ExecuteDeleteAsync(options?.CancellationToken ?? default);
    }

    /// <inheritdoc/>
    public virtual async Task<int> DeleteAsync(TModel model, ContextOptions options = null)
    {
        var entity = mapper.ToEntity(model);

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
        return await context.SaveChangesAsync(options?.CancellationToken ?? default);
    }

    /// <inheritdoc/>
    public virtual async Task<int> DeleteAsync(IEnumerable<TModel> models, ContextOptions options = null)
    {
        var entities = models.Select(mapper.ToEntity).ToList();

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
        return await context.SaveChangesAsync(options?.CancellationToken ?? default);
    }

    /// <inheritdoc/>
    public virtual async Task<int> DeleteAsync(Expression<Func<TModel, bool>> predicate, ContextOptions options = null)
    {
        var mappedPredicate = mapper.MapPredicate(predicate);
        using var context = GetContext(options);
        return await context.Set<TEntity>().Where(mappedPredicate).ExecuteDeleteAsync(options?.CancellationToken ?? default);
    }

    #endregion Delete

    #region Insert

    /// <inheritdoc/>
    public virtual TModel Insert(TModel model, ContextOptions options = null)
    {
        var entity = mapper.ToEntity(model);
        using var context = GetContext(options);
        context.Set<TEntity>().Add(entity);
        context.SaveChanges();
        return mapper.ToModel(entity);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<TModel> Insert(IEnumerable<TModel> models, ContextOptions options = null)
    {
        var entities = models.Select(mapper.ToEntity).ToList();
        using var context = GetContext(options);
        context.Set<TEntity>().AddRange(entities);
        context.SaveChanges();
        return entities.Select(mapper.ToModel).ToList();
    }

    /// <inheritdoc/>
    public virtual async Task<TModel> InsertAsync(TModel model, ContextOptions options = null)
    {
        var cancellationToken = options?.CancellationToken ?? default;
        var entity = mapper.ToEntity(model);
        using var context = GetContext(options);
        await context.Set<TEntity>().AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.ToModel(entity);
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<TModel>> InsertAsync(IEnumerable<TModel> models, ContextOptions options = null)
    {
        var cancellationToken = options?.CancellationToken ?? default;
        var entities = models.Select(mapper.ToEntity).ToList();
        using var context = GetContext(options);
        await context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entities.Select(mapper.ToModel).ToList();
    }

    #endregion Insert

    #region Update

    /// <inheritdoc/>
    public virtual TModel Update(TModel model, ContextOptions options = null)
    {
        var entity = mapper.ToEntity(model);

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

        context.SaveChanges();
        return mapper.ToModel(entity);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<TModel> Update(IEnumerable<TModel> models, ContextOptions options = null)
    {
        var entities = models.Select(mapper.ToEntity).ToList();

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
        return entities.Select(mapper.ToModel).ToList();
    }

    /// <inheritdoc/>
    public virtual async Task<TModel> UpdateAsync(TModel model, ContextOptions options = null)
    {
        var entity = mapper.ToEntity(model);

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

        await context.SaveChangesAsync(options?.CancellationToken ?? default);
        return mapper.ToModel(entity);
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<TModel>> UpdateAsync(IEnumerable<TModel> models, ContextOptions options = null)
    {
        var entities = models.Select(mapper.ToEntity).ToList();
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

        await context.SaveChangesAsync(options?.CancellationToken ?? default);
        return entities.Select(mapper.ToModel).ToList();
    }

    /// <inheritdoc/>
    public virtual int Update(Expression<Func<SetPropertyCalls<TModel>, SetPropertyCalls<TModel>>> setPropertyCalls, ContextOptions options = null)
    {
        var mappedSetPropertyCalls = mapper.MapSetPropertyCalls(setPropertyCalls);
        using var context = GetContext(options);
        return context.Set<TEntity>().ExecuteUpdate(mappedSetPropertyCalls);
    }

    /// <inheritdoc/>
    public virtual int Update(Expression<Func<TModel, bool>> predicate, Expression<Func<SetPropertyCalls<TModel>, SetPropertyCalls<TModel>>> setPropertyCalls, ContextOptions options = null)
    {
        var mappedPredicate = mapper.MapPredicate(predicate);
        var mappedSetPropertyCalls = mapper.MapSetPropertyCalls(setPropertyCalls);
        using var context = GetContext(options);
        return context.Set<TEntity>().Where(mappedPredicate).ExecuteUpdate(mappedSetPropertyCalls);
    }

    /// <inheritdoc/>
    public virtual async Task<int> UpdateAsync(Expression<Func<SetPropertyCalls<TModel>, SetPropertyCalls<TModel>>> setPropertyCalls, ContextOptions options = null)
    {
        var mappedSetPropertyCalls = mapper.MapSetPropertyCalls(setPropertyCalls);
        using var context = GetContext(options);
        return await context.Set<TEntity>().ExecuteUpdateAsync(mappedSetPropertyCalls, options?.CancellationToken ?? default);
    }

    /// <inheritdoc/>
    public virtual async Task<int> UpdateAsync(Expression<Func<TModel, bool>> predicate, Expression<Func<SetPropertyCalls<TModel>, SetPropertyCalls<TModel>>> setPropertyCalls, ContextOptions options = null)
    {
        var mappedPredicate = mapper.MapPredicate(predicate);
        var mappedSetPropertyCalls = mapper.MapSetPropertyCalls(setPropertyCalls);
        using var context = GetContext(options);
        return await context.Set<TEntity>().Where(mappedPredicate).ExecuteUpdateAsync(mappedSetPropertyCalls, options?.CancellationToken ?? default);
    }

    #endregion Update

    #endregion IRepository<TModel> Members

    #region IEntityFrameworkRepository<TEntity> Members

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

    #endregion IEntityFrameworkRepository<TEntity> Members

    private IQueryable<TEntity> BuildBaseQuery(DbContext context, SearchOptions<TEntity> options)
    {
        var query = context.Set<TEntity>().AsNoTracking();

        if (GlobalRepositoryOptions.TagWithCallSite && options.TagWithCallSite)
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

    #region Experimental

    // Mostly works, but there are some issues mapping filtered includes.
    // As such, I've opted to use TEntity instead of TModel for the SearchOptions

    //private IQueryable<TEntity> BuildBaseQuery(DbContext context, SearchOptions<TModel> options)
    //{
    //    var query = context.Set<TEntity>().AsNoTracking();

    //    if (options.TagWithCallSite)
    //    {
    //        query = query.TagWith(options.CallSiteTag);
    //    }

    //    if (!options.Tags.IsNullOrEmpty())
    //    {
    //        foreach (string tag in options.Tags)
    //        {
    //            query = query.TagWith(tag);
    //        }
    //    }

    //    if (options.Include is not null)
    //    {
    //        var mappedInclude = MapInclude(options.Include);
    //        query = mappedInclude(query);

    //        if (options.SplitQuery)
    //        {
    //            query = query.AsSplitQuery();
    //        }
    //    }

    //    if (options.Query is not null)
    //    {
    //        var mappedPredicate = mapper.MapPredicate(options.Query);
    //        query = query.Where(mappedPredicate);
    //    }

    //    if (options.OrderBy is not null)
    //    {
    //        var mappedOrderBy = MapOrderBy(options.OrderBy);
    //        query = mappedOrderBy(query);
    //    }

    //    return query;
    //}

    //private static IQueryable<T> ApplyPaging<T>(IQueryable<T> query, SearchOptions<TModel> options)
    //{
    //    if (options.PageSize > 0 && options.PageNumber > 0)
    //    {
    //        query = query
    //            .Skip((options.PageNumber - 1) * options.PageSize)
    //            .Take(options.PageSize);
    //    }

    //    return query;
    //}

    #endregion Experimental
}
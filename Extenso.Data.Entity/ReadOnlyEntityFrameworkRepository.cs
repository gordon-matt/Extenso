using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Extenso.Collections;
using Extenso.Collections.Generic;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Extenso.Data.Entity;

public class ReadOnlyEntityFrameworkRepository<TEntity> : IReadOnlyRepository<TEntity>, IEntityFrameworkRepository
    where TEntity : class, IEntity
{
    #region Non-Public Members

    protected IDbContextFactory contextFactory;

    #endregion Non-Public Members

    #region Constructor

    public ReadOnlyEntityFrameworkRepository(IDbContextFactory contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    #endregion Constructor

    #region IReadOnlyRepository<TEntity> Members

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

    public virtual async IAsyncEnumerable<TEntity> StreamAsync(
        SearchOptions<TEntity> options,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var context = GetContext(options);
        var query = ApplyPaging(
            BuildBaseQuery(context, options),
            options);

        await foreach (var entity in query.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            yield return entity;
        }
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
        var cancellationToken = options?.CancellationToken ?? default;

        using var context = GetContext(options);
        var query = BuildBaseQuery(context, options);
        int totalCount = await query.CountAsync(cancellationToken);
        query = ApplyPaging(query, options);

        return new PagedList<TEntity>(
            await query.ToListAsync(cancellationToken),
            options.PageNumber > 0 ? options.PageNumber - 1 : 1,
            options.PageSize,
            totalCount);
    }

    /// <inheritdoc/>
    public virtual async Task<IPagedCollection<TResult>> FindAsync<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection)
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
        return await query.FirstOrDefaultAsync(options?.CancellationToken ?? default);
    }

    /// <inheritdoc/>
    public virtual async Task<TResult> FindOneAsync<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection)
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
    public virtual int Count(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return context.Set<TEntity>().AsNoTracking().Count(predicate);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().AsNoTracking().CountAsync(options?.CancellationToken ?? default);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().AsNoTracking().CountAsync(predicate, options?.CancellationToken ?? default);
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
        return await context.Set<TEntity>().AsNoTracking().LongCountAsync(options?.CancellationToken ?? default);
    }

    /// <inheritdoc/>
    public virtual async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().AsNoTracking().LongCountAsync(predicate, options?.CancellationToken ?? default);
    }

    #endregion Count

    #region Exists

    /// <inheritdoc/>
    public bool Exists(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return context.Set<TEntity>().AsNoTracking().Any(predicate);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().AsNoTracking().AnyAsync(predicate, options?.CancellationToken ?? default);
    }

    #endregion Exists

    #endregion IRepository<TEntity> Members

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

    protected IQueryable<TEntity> BuildBaseQuery(DbContext context, SearchOptions<TEntity> options)
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

    protected static IQueryable<T> ApplyPaging<T>(IQueryable<T> query, SearchOptions<TEntity> options)
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
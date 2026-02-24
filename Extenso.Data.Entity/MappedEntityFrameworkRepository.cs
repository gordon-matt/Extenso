using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Extenso.Data.Entity;

public class MappedEntityFrameworkRepository<TModel, TEntity> : ReadOnlyMappedEntityFrameworkRepository<TModel, TEntity>, IMappedRepository<TModel, TEntity>, IEntityFrameworkRepository
    where TModel : class
    where TEntity : class, IEntity
{
    public MappedEntityFrameworkRepository(
        IDbContextFactory contextFactory,
        IEntityModelMapper<TEntity, TModel> mapper)
        : base(contextFactory, mapper)
    {
    }

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
    public virtual int Update(Expression<Action<UpdateSettersBuilder<TModel>>> setPropertyCalls, ContextOptions options = null)
    {
        var mappedSetPropertyCalls = mapper.MapSetPropertyCalls(setPropertyCalls);
        using var context = GetContext(options);
        return context.Set<TEntity>().ExecuteUpdate(mappedSetPropertyCalls.Compile());
    }

    /// <inheritdoc/>
    public virtual int Update(Expression<Func<TModel, bool>> predicate, Expression<Action<UpdateSettersBuilder<TModel>>> setPropertyCalls, ContextOptions options = null)
    {
        var mappedPredicate = mapper.MapPredicate(predicate);
        var mappedSetPropertyCalls = mapper.MapSetPropertyCalls(setPropertyCalls).Compile();
        using var context = GetContext(options);
        return context.Set<TEntity>().Where(mappedPredicate).ExecuteUpdate(mappedSetPropertyCalls);
    }

    /// <inheritdoc/>
    public virtual async Task<int> UpdateAsync(Expression<Action<UpdateSettersBuilder<TModel>>> setPropertyCalls, ContextOptions options = null)
    {
        var mappedSetPropertyCalls = mapper.MapSetPropertyCalls(setPropertyCalls).Compile();
        using var context = GetContext(options);
        return await context.Set<TEntity>().ExecuteUpdateAsync(mappedSetPropertyCalls, options?.CancellationToken ?? default);
    }

    /// <inheritdoc/>
    public virtual async Task<int> UpdateAsync(Expression<Func<TModel, bool>> predicate, Expression<Action<UpdateSettersBuilder<TModel>>> setPropertyCalls, ContextOptions options = null)
    {
        var mappedPredicate = mapper.MapPredicate(predicate);
        var mappedSetPropertyCalls = mapper.MapSetPropertyCalls(setPropertyCalls).Compile();
        using var context = GetContext(options);
        return await context.Set<TEntity>().Where(mappedPredicate).ExecuteUpdateAsync(mappedSetPropertyCalls, options?.CancellationToken ?? default);
    }

    #endregion Update
}
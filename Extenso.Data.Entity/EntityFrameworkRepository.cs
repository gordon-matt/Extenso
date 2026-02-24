using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Extenso.Collections;
using Extenso.Collections.Generic;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;

namespace Extenso.Data.Entity;

public class EntityFrameworkRepository<TEntity> : ReadOnlyEntityFrameworkRepository<TEntity>, IRepository<TEntity>, IEntityFrameworkRepository
    where TEntity : class, IEntity
{
    public EntityFrameworkRepository(IDbContextFactory contextFactory)
        : base(contextFactory)
    {
    }

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
        return context.Set<TEntity>().ExecuteDelete();
    }

    /// <inheritdoc/>
    public virtual async Task<int> DeleteAllAsync(ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().ExecuteDeleteAsync(options?.CancellationToken ?? default);
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
        return await context.SaveChangesAsync(options?.CancellationToken ?? default);
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
        return await context.SaveChangesAsync(options?.CancellationToken ?? default);
    }

    /// <inheritdoc/>
    public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().Where(predicate).ExecuteDeleteAsync(options?.CancellationToken ?? default);
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
        var cancellationToken = options?.CancellationToken ?? default;
        using var context = GetContext(options);
        await context.Set<TEntity>().AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities, ContextOptions options = null)
    {
        var cancellationToken = options?.CancellationToken ?? default;
        using var context = GetContext(options);
        await context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entities;
    }

    #endregion Insert

    #region Update

    /// <inheritdoc/>
    public virtual TEntity Update(TEntity entity, ContextOptions options = null)
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

    /// <inheritdoc/>
    public virtual IEnumerable<TEntity> Update(IEnumerable<TEntity> entities, ContextOptions options = null)
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

    /// <inheritdoc/>
    public virtual async Task<TEntity> UpdateAsync(TEntity entity, ContextOptions options = null)
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

        await context.SaveChangesAsync(options?.CancellationToken ?? default);
        return entity;
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities, ContextOptions options = null)
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

        await context.SaveChangesAsync(options?.CancellationToken ?? default);
        return entities;
    }

    /// <inheritdoc/>
    public virtual int Update(Expression<Action<UpdateSettersBuilder<TEntity>>> setPropertyCalls, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return context.Set<TEntity>().ExecuteUpdate(setPropertyCalls.Compile());
    }

    /// <inheritdoc/>
    public virtual int Update(Expression<Func<TEntity, bool>> predicate, Expression<Action<UpdateSettersBuilder<TEntity>>> setPropertyCalls, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return context.Set<TEntity>().Where(predicate).ExecuteUpdate(setPropertyCalls.Compile());
    }

    /// <inheritdoc/>
    public virtual async Task<int> UpdateAsync(Expression<Action<UpdateSettersBuilder<TEntity>>> setPropertyCalls, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().ExecuteUpdateAsync(setPropertyCalls.Compile(), options?.CancellationToken ?? default);
    }

    /// <inheritdoc/>
    public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Action<UpdateSettersBuilder<TEntity>>> setPropertyCalls, ContextOptions options = null)
    {
        using var context = GetContext(options);
        return await context.Set<TEntity>().Where(predicate).ExecuteUpdateAsync(setPropertyCalls.Compile(), options?.CancellationToken ?? default);
    }

    #endregion Update
}
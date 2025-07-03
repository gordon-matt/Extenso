using System.Linq.Expressions;
using Extenso.Collections.Generic;

namespace Extenso.Data.Entity;

public interface IMappedRepository<TModel, TEntity>
    where TModel : class
    where TEntity : class, IEntity
{
    IRepositoryConnection<TModel> OpenConnection(ContextOptions options = null);

    /// <inheritdoc/>
    IRepositoryConnection<TModel> UseConnection<TOther>(IRepositoryConnection<TOther> connection)
        where TOther : class;

    #region Find

    /// <inheritdoc/>
    IPagedCollection<TModel> Find(SearchOptions<TEntity> options);

    /// <inheritdoc/>
    IPagedCollection<TResult> Find<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection);

    /// <inheritdoc/>
    Task<IPagedCollection<TModel>> FindAsync(SearchOptions<TEntity> options);

    /// <inheritdoc/>
    Task<IPagedCollection<TResult>> FindAsync<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection);

    /// <inheritdoc/>
    TModel FindOne(params object[] keyValues);

    /// <inheritdoc/>
    TModel FindOne(SearchOptions<TEntity> options);

    /// <inheritdoc/>
    TResult FindOne<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection);

    /// <inheritdoc/>
    Task<TModel> FindOneAsync(params object[] keyValues);

    /// <inheritdoc/>
    Task<TModel> FindOneAsync(SearchOptions<TEntity> options);

    /// <inheritdoc/>
    Task<TResult> FindOneAsync<TResult>(SearchOptions<TEntity> options, Expression<Func<TEntity, TResult>> projection);

    #endregion Find

    #region Count

    /// <inheritdoc/>
    int Count(ContextOptions options = null);

    /// <inheritdoc/>
    int Count(Expression<Func<TModel, bool>> predicate, ContextOptions options = null);

    /// <inheritdoc/>
    Task<int> CountAsync(ContextOptions options = null);

    /// <inheritdoc/>
    Task<int> CountAsync(Expression<Func<TModel, bool>> predicate, ContextOptions options = null);

    /// <inheritdoc/>
    long LongCount(ContextOptions options = null);

    /// <inheritdoc/>
    long LongCount(Expression<Func<TModel, bool>> predicate, ContextOptions options = null);

    /// <inheritdoc/>
    Task<long> LongCountAsync(ContextOptions options = null);

    /// <inheritdoc/>
    Task<long> LongCountAsync(Expression<Func<TModel, bool>> predicate, ContextOptions options = null);

    #endregion Count

    #region Delete

    /// <inheritdoc/>
    int Delete(TModel model, ContextOptions options = null);

    /// <inheritdoc/>
    int Delete(IEnumerable<TModel> models, ContextOptions options = null);

    /// <inheritdoc/>
    int Delete(Expression<Func<TModel, bool>> predicate, ContextOptions options = null);

    /// <inheritdoc/>
    int DeleteAll(ContextOptions options = null);

    /// <inheritdoc/>
    Task<int> DeleteAllAsync(ContextOptions options = null);

    /// <inheritdoc/>
    Task<int> DeleteAsync(TModel model, ContextOptions options = null);

    /// <inheritdoc/>
    Task<int> DeleteAsync(IEnumerable<TModel> models, ContextOptions options = null);

    /// <inheritdoc/>
    Task<int> DeleteAsync(Expression<Func<TModel, bool>> predicate, ContextOptions options = null);

    #endregion Delete

    #region Insert

    /// <inheritdoc/>
    TModel Insert(TModel model, ContextOptions options = null);

    /// <inheritdoc/>
    IEnumerable<TModel> Insert(IEnumerable<TModel> models, ContextOptions options = null);

    /// <inheritdoc/>
    Task<TModel> InsertAsync(TModel model, ContextOptions options = null);

    /// <inheritdoc/>
    Task<IEnumerable<TModel>> InsertAsync(IEnumerable<TModel> models, ContextOptions options = null);

    #endregion Insert

    #region Update

    /// <inheritdoc/>
    TModel Update(TModel model, ContextOptions options = null);

    /// <inheritdoc/>
    IEnumerable<TModel> Update(IEnumerable<TModel> models, ContextOptions options = null);

    /// <inheritdoc/>
    int Update(Expression<Func<TModel, TModel>> updateFactory, ContextOptions options = null);

    /// <inheritdoc/>
    int Update(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TModel>> updateFactory, ContextOptions options = null);

    /// <inheritdoc/>
    Task<TModel> UpdateAsync(TModel model, ContextOptions options = null);

    /// <inheritdoc/>
    Task<IEnumerable<TModel>> UpdateAsync(IEnumerable<TModel> models, ContextOptions options = null);

    /// <inheritdoc/>
    Task<int> UpdateAsync(Expression<Func<TModel, TModel>> updateFactory, ContextOptions options = null);

    #endregion Update
}
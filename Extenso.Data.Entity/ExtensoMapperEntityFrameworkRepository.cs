using System.Linq.Expressions;
using Extenso.Mapping;
using Microsoft.Extensions.Logging;

namespace Extenso.Data.Entity;

public class ExtensoMapperEntityFrameworkRepository<TModel, TEntity> : MappedEntityFrameworkRepository<TModel, TEntity>
    where TModel : class
    where TEntity : class, IEntity
{
    public ExtensoMapperEntityFrameworkRepository(IDbContextFactory contextFactory, ILoggerFactory loggerFactory)
        : base(contextFactory, loggerFactory)
    {
    }

    /// <inheritdoc/>
    protected override Expression<Func<TEntity, TProperty>> MapInclude<TProperty>(Expression<Func<TModel, TProperty>> includeExpression) =>
        ExtensoMapper.MapInclude<TModel, TEntity, TProperty>(includeExpression);

    /// <inheritdoc/>
    protected override Func<IQueryable<TEntity>, IQueryable<TEntity>> MapInclude(
        Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> includeExpression) =>
        ExtensoMapper.MapInclude<TModel, TEntity>(includeExpression);

    /// <inheritdoc/>
    protected override Func<IQueryable<TEntity>, IQueryable<TEntity>> MapOrderBy(
        Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> includeExpression) =>
        ExtensoMapper.MapOrderBy<TModel, TEntity>(includeExpression);

    /// <inheritdoc/>
    protected override Expression<Func<TEntity, bool>> MapPredicate(Expression<Func<TModel, bool>> predicate) =>
        ExtensoMapper.MapPredicate<TModel, TEntity>(predicate);

    /// <inheritdoc/>
    protected override Expression<Func<TEntity, TResult>> MapProjection<TResult>(
        Expression<Func<TModel, TResult>> projectionExpression) =>
        ExtensoMapper.MapProjection<TModel, TEntity, TResult>(projectionExpression);

    /// <inheritdoc/>
    protected override IQueryable<TModel> MapQuery(IQueryable<TEntity> query) =>
        ExtensoMapper.MapQuery<TEntity, TModel>(query);

    /// <inheritdoc/>
    protected override Expression<Func<TEntity, TEntity>> MapUpdate(Expression<Func<TModel, TModel>> updateExpression) =>
        ExtensoMapper.MapUpdate<TModel, TEntity>(updateExpression);

    /// <inheritdoc/>
    protected override TEntity ToEntity(TModel model) => ExtensoMapper.Map<TModel, TEntity>(model);

    /// <inheritdoc/>
    protected override TModel ToModel(TEntity entity) => ExtensoMapper.Map<TEntity, TModel>(entity);
}
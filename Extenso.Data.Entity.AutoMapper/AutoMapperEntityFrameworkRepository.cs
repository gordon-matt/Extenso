using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.Extensions.Logging;

namespace Extenso.Data.Entity.AutoMapper;

public class AutoMapperEntityFrameworkRepository<TModel, TEntity> : MappedEntityFrameworkRepository<TModel, TEntity>
    where TModel : class
    where TEntity : class, IEntity
{
    private readonly IMapper mapper;

    public AutoMapperEntityFrameworkRepository(IDbContextFactory contextFactory, ILoggerFactory loggerFactory, IMapper mapper)
        : base(contextFactory, loggerFactory)
    {
        this.mapper = mapper;
    }

    /// <inheritdoc/>
    protected override Expression<Func<TEntity, TProperty>> MapInclude<TProperty>(Expression<Func<TModel, TProperty>> includeExpression) =>
        mapper.MapExpressionAsInclude<Expression<Func<TEntity, TProperty>>>(includeExpression);

    /// <inheritdoc/>
    protected override Func<IQueryable<TEntity>, IQueryable<TEntity>> MapInclude(
        Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> includeExpression) =>
        mapper.MapExpressionAsInclude<TModel, TEntity>(includeExpression);

    /// <inheritdoc/>
    protected override Func<IQueryable<TEntity>, IQueryable<TEntity>> MapOrderBy(
        Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> orderByExpression) =>
        mapper.MapExpressionAsOrderBy<TModel, TEntity>(orderByExpression);

    /// <inheritdoc/>
    protected override Expression<Func<TEntity, bool>> MapPredicate(Expression<Func<TModel, bool>> predicate) =>
        mapper.MapExpression<Expression<Func<TEntity, bool>>>(predicate);

    /// <inheritdoc/>
    protected override Expression<Func<TEntity, TResult>> MapProjection<TResult>(
        Expression<Func<TModel, TResult>> projectionExpression) =>
        mapper.MapExpression<Expression<Func<TEntity, TResult>>>(projectionExpression);

    /// <inheritdoc/>
    protected override IQueryable<TModel> MapQuery(IQueryable<TEntity> query) =>
        mapper.ProjectTo<TModel>(query);

    /// <inheritdoc/>
    protected override Expression<Func<TEntity, TEntity>> MapUpdate(Expression<Func<TModel, TModel>> updateExpression) =>
        mapper.MapExpression<Expression<Func<TEntity, TEntity>>>(updateExpression);

    /// <inheritdoc/>
    protected override TEntity ToEntity(TModel model) => mapper.Map<TModel, TEntity>(model);

    /// <inheritdoc/>
    protected override TModel ToModel(TEntity entity) => mapper.Map<TEntity, TModel>(entity);
}
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

    public override TEntity ToEntity(TModel model) => mapper.Map<TModel, TEntity>(model);

    public override TModel ToModel(TEntity entity) => mapper.Map<TEntity, TModel>(entity);

    public override Expression<Func<TEntity, TProperty>> MapIncludeExpression<TProperty>(Expression<Func<TModel, TProperty>> includeExpression) =>
        mapper.MapExpressionAsInclude<Expression<Func<TEntity, TProperty>>>(includeExpression);

    public override Expression<Func<TEntity, bool>> MapPredicateExpression(Expression<Func<TModel, bool>> predicate) =>
        mapper.MapExpression<Expression<Func<TEntity, bool>>>(predicate);

    public override Expression<Func<TEntity, TEntity>> MapUpdateExpression(Expression<Func<TModel, TModel>> updateExpression) =>
        mapper.MapExpression<Expression<Func<TEntity, TEntity>>>(updateExpression);
}
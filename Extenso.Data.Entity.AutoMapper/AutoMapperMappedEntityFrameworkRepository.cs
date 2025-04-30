using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Extenso.Data.Entity.AutoMapper;

public class AutoMapperMappedEntityFrameworkRepository<TModel, TEntity> : MappedEntityFrameworkRepository<TModel, TEntity>
    where TModel : class
    where TEntity : class, IEntity
{
    private readonly IMapper mapper;

    public AutoMapperMappedEntityFrameworkRepository(IDbContextFactory contextFactory, ILoggerFactory loggerFactory, IMapper mapper)
        : base(contextFactory, loggerFactory)
    {
        this.mapper = mapper;
    }

    public override TEntity ToEntity(TModel model) => mapper.Map<TModel, TEntity>(model);

    public override TModel ToModel(TEntity entity) => mapper.Map<TEntity, TModel>(entity);

    public override Expression<Func<TEntity, object>> MapIncludeExpression(Expression<Func<TModel, dynamic>> includeExpression) =>
        mapper.Map<Expression<Func<TEntity, object>>>(includeExpression);

    public override Expression<Func<TEntity, bool>> MapPredicateExpression(Expression<Func<TModel, bool>> predicate) =>
        mapper.Map<Expression<Func<TEntity, bool>>>(predicate);

    public override Expression<Func<TEntity, TEntity>> MapUpdateExpression(Expression<Func<TModel, TModel>> updateExpression) =>
        mapper.Map<Expression<Func<TEntity, TEntity>>>(updateExpression);
}
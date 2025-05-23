﻿using System.Linq.Expressions;
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

    public override TEntity ToEntity(TModel model) => ExtensoMapper.Map<TModel, TEntity>(model);

    public override TModel ToModel(TEntity entity) => ExtensoMapper.Map<TEntity, TModel>(entity);

    public override IQueryable<TModel> MapQuery(IQueryable<TEntity> query) =>
        ExtensoMapper.MapQuery<TEntity, TModel>(query);

    public override Expression<Func<TEntity, bool>> MapPredicate(Expression<Func<TModel, bool>> predicate) =>
        ExtensoMapper.MapPredicate<TModel, TEntity>(predicate);

    public override Expression<Func<TEntity, TProperty>> MapInclude<TProperty>(Expression<Func<TModel, TProperty>> includeExpression) =>
        ExtensoMapper.MapInclude<TModel, TEntity, TProperty>(includeExpression);

    public override Expression<Func<TEntity, TEntity>> MapUpdate(Expression<Func<TModel, TModel>> updateExpression) =>
        ExtensoMapper.MapUpdate<TModel, TEntity>(updateExpression);
}
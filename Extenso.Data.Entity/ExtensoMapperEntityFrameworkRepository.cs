using System;
using System.Linq;
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

    public override TEntity ToEntity(TModel model) => ExtensoMapper.Map<TModel, TEntity>(model);

    public override TModel ToModel(TEntity entity) => ExtensoMapper.Map<TEntity, TModel>(entity);

    public override IQueryable<TModel> MapQueryable(IQueryable<TEntity> query) =>
        ExtensoMapper.MapQuery<TEntity, TModel>(query);

    public override Expression<Func<TEntity, TProperty>> MapIncludeExpression<TProperty>(Expression<Func<TModel, TProperty>> includeExpression) =>
        ExtensoMapper.MapInclude<TModel, TEntity, TProperty>(includeExpression);

    public override Expression<Func<TEntity, bool>> MapPredicateExpression(Expression<Func<TModel, bool>> predicate) =>
        ExtensoMapper.MapPredicate<TModel, TEntity>(predicate);

    public override Expression<Func<TEntity, TEntity>> MapUpdateExpression(Expression<Func<TModel, TModel>> updateExpression) =>
        ExtensoMapper.MapUpdateExpression<TModel, TEntity>(updateExpression);

    //public override Expression<Func<TEntity, TProperty>> MapIncludeExpression<TProperty>(Expression<Func<TModel, TProperty>> includeExpression) =>
    //    throw new NotImplementedException();

    //public override Expression<Func<TEntity, bool>> MapPredicateExpression(Expression<Func<TModel, bool>> predicate) =>
    //    throw new NotImplementedException();

    //public override Expression<Func<TEntity, TEntity>> MapUpdateExpression(Expression<Func<TModel, TModel>> updateExpression) =>
    //    throw new NotImplementedException();
}
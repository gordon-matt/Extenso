using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore.Query;

namespace Extenso.Data.Entity.AutoMapper;

public class AutoMapperEntityModelMapper<TEntity, TModel> : IEntityModelMapper<TEntity, TModel>
{
    private readonly IMapper mapper;

    public AutoMapperEntityModelMapper(IMapper mapper)
    {
        this.mapper = mapper;
    }

    public Expression<Func<TEntity, TProperty>> MapInclude<TProperty>(Expression<Func<TModel, TProperty>> includeExpression) =>
        mapper.MapExpressionAsInclude<Expression<Func<TEntity, TProperty>>>(includeExpression);

    public Func<IQueryable<TEntity>, IQueryable<TEntity>> MapInclude(
        Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> includeExpression) =>
        mapper.MapExpressionAsInclude<TModel, TEntity>(includeExpression);

    public Func<IQueryable<TEntity>, IQueryable<TEntity>> MapOrderBy(
        Expression<Func<IQueryable<TModel>, IQueryable<TModel>>> orderByExpression) =>
        mapper.MapExpressionAsOrderBy<TModel, TEntity>(orderByExpression);

    public Expression<Func<TEntity, bool>> MapPredicate(Expression<Func<TModel, bool>> predicate) =>
        mapper.MapExpression<Expression<Func<TEntity, bool>>>(predicate);

    public Expression<Func<TEntity, TResult>> MapProjection<TResult>(
        Expression<Func<TModel, TResult>> projectionExpression) =>
        mapper.MapExpression<Expression<Func<TEntity, TResult>>>(projectionExpression);

    public IQueryable<TModel> MapQuery(IQueryable<TEntity> query) =>
        mapper.ProjectTo<TModel>(query);

    public Expression<Func<TEntity, TEntity>> MapUpdate(Expression<Func<TModel, TModel>> updateExpression) =>
        mapper.MapExpression<Expression<Func<TEntity, TEntity>>>(updateExpression);

    public Expression<Action<UpdateSettersBuilder<TEntity>>> MapSetPropertyCalls(
        Expression<Action<UpdateSettersBuilder<TModel>>> setPropertyCalls) =>
        mapper.MapExpression<Expression<Action<UpdateSettersBuilder<TEntity>>>>(setPropertyCalls);

    public TEntity ToEntity(TModel model) => mapper.Map<TModel, TEntity>(model);

    public TModel ToModel(TEntity entity) => mapper.Map<TEntity, TModel>(entity);
}
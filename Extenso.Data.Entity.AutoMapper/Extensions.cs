using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore.Query;

namespace Extenso.Data.Entity.AutoMapper;

public static class Extensions
{
    extension(IMapper mapper)
    {
        public Func<IQueryable<TDest>, IIncludableQueryable<TDest, object>> MapExpressionAsInclude<TSource, TDest>(
            Expression<Func<IQueryable<TSource>, IQueryable<TSource>>> includeFunc) => includeFunc == null
                ? throw new ArgumentNullException(nameof(includeFunc))
                : mapper.MapExpressionAsInclude<Expression<Func<IQueryable<TDest>, IIncludableQueryable<TDest, object>>>>(includeFunc).Compile();

        public Func<IQueryable<TDest>, IOrderedQueryable<TDest>> MapExpressionAsOrderBy<TSource, TDest>(
            Expression<Func<IQueryable<TSource>, IQueryable<TSource>>> orderByFunc) => orderByFunc == null
                ? throw new ArgumentNullException(nameof(orderByFunc))
                : mapper.MapExpression<Expression<Func<IQueryable<TDest>, IOrderedQueryable<TDest>>>>(orderByFunc).Compile();
    }
}
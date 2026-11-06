using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore.Query;

namespace Extenso.Data.Entity.AutoMapper;

/// <summary>
/// Provides extension methods for values.
/// </summary>
public static class Extensions
{
    extension(IMapper mapper)
    {
        /// <summary>
        /// Maps the expression as include.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDest">The type of the dest.</typeparam>
        /// <param name="includeFunc">The include func.</param>
        /// <returns>The expression as include.</returns>
        public Func<IQueryable<TDest>, IIncludableQueryable<TDest, object>> MapExpressionAsInclude<TSource, TDest>(
            Expression<Func<IQueryable<TSource>, IQueryable<TSource>>> includeFunc) => includeFunc == null
                ? throw new ArgumentNullException(nameof(includeFunc))
                : mapper.MapExpressionAsInclude<Expression<Func<IQueryable<TDest>, IIncludableQueryable<TDest, object>>>>(includeFunc).Compile();

        /// <summary>
        /// Maps the expression as order by.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDest">The type of the dest.</typeparam>
        /// <param name="orderByFunc">The order by func.</param>
        /// <returns>The expression as order.</returns>
        public Func<IQueryable<TDest>, IOrderedQueryable<TDest>> MapExpressionAsOrderBy<TSource, TDest>(
            Expression<Func<IQueryable<TSource>, IQueryable<TSource>>> orderByFunc) => orderByFunc == null
                ? throw new ArgumentNullException(nameof(orderByFunc))
                : mapper.MapExpression<Expression<Func<IQueryable<TDest>, IOrderedQueryable<TDest>>>>(orderByFunc).Compile();
    }
}
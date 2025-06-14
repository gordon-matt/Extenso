﻿using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore.Query;

namespace Extenso.Data.Entity.AutoMapper;

public static class Extensions
{
    public static Func<IQueryable<TDest>, IIncludableQueryable<TDest, object>> MapExpressionAsInclude<TSource, TDest>(
        this IMapper mapper,
        Expression<Func<IQueryable<TSource>, IQueryable<TSource>>> includeFunc) => includeFunc == null
            ? throw new ArgumentNullException(nameof(includeFunc))
            : mapper.MapExpressionAsInclude<Expression<Func<IQueryable<TDest>, IIncludableQueryable<TDest, object>>>>(includeFunc).Compile();

    public static Func<IQueryable<TDest>, IOrderedQueryable<TDest>> MapExpressionAsOrderBy<TSource, TDest>(
        this IMapper mapper,
        Expression<Func<IQueryable<TSource>, IQueryable<TSource>>> orderByFunc) => orderByFunc == null
            ? throw new ArgumentNullException(nameof(orderByFunc))
            : mapper.MapExpression<Expression<Func<IQueryable<TDest>, IOrderedQueryable<TDest>>>>(orderByFunc).Compile();
}
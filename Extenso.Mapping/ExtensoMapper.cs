using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;

namespace Extenso.Mapping;

public static class ExtensoMapper
{
    public static readonly ConcurrentDictionary<TypePair, Func<object, object>> mappings = new();

    public static void Register<TSource, TDestination>(Func<TSource, TDestination> mapFunc)
    {
        var key = new TypePair(typeof(TSource), typeof(TDestination));
        mappings[key] = (source) => mapFunc((TSource)source);
    }

    public static TDestination Map<TSource, TDestination>(TSource source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var key = new TypePair(typeof(TSource), typeof(TDestination));

        return mappings.TryGetValue(key, out var mapFunc)
            ? (TDestination)mapFunc(source)
            : throw new InvalidOperationException($"Mapping from {typeof(TSource)} to {typeof(TDestination)} is not registered.");
    }

    public static object Map(object source, Type sourceType, Type destinationType)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(sourceType);
        ArgumentNullException.ThrowIfNull(destinationType);

        var key = new TypePair(sourceType, destinationType);

        return mappings.TryGetValue(key, out var mapFunc)
            ? mapFunc(source)
            : throw new InvalidOperationException($"Mapping from {sourceType} to {destinationType} is not registered.");
    }
}

#region Work in Progress

//public static class ExtensoMapper
//{
//    private static readonly ConcurrentDictionary<TypePair, Func<object, object>> mappings = new();

//    public static void Register<TSource, TDestination>(Func<TSource, TDestination> mapFunc)
//    {
//        var key = new TypePair(typeof(TSource), typeof(TDestination));
//        mappings[key] = source => mapFunc((TSource)source);
//    }

//    public static TDestination Map<TSource, TDestination>(TSource source)
//    {
//        ArgumentNullException.ThrowIfNull(source);

//        var key = new TypePair(typeof(TSource), typeof(TDestination));

//        return mappings.TryGetValue(key, out var mapFunc)
//            ? (TDestination)mapFunc(source)
//            : throw new InvalidOperationException($"Mapping from {typeof(TSource)} to {typeof(TDestination)} is not registered.");
//    }

//    public static object Map(object source, Type sourceType, Type destinationType)
//    {
//        ArgumentNullException.ThrowIfNull(source);
//        ArgumentNullException.ThrowIfNull(sourceType);
//        ArgumentNullException.ThrowIfNull(destinationType);

//        var key = new TypePair(sourceType, destinationType);

//        return mappings.TryGetValue(key, out var mapFunc)
//            ? mapFunc(source)
//            : throw new InvalidOperationException($"Mapping from {sourceType} to {destinationType} is not registered.");
//    }

//    //Cache for expression mappings
//    private static readonly ConcurrentDictionary<TypePair, Delegate> expressionMappings = new();

//    public static Expression<Func<TDestination, bool>> MapExpression<TSource, TDestination>(
//        Expression<Func<TSource, bool>> predicate)
//    {
//        ArgumentNullException.ThrowIfNull(predicate);

//        var key = new TypePair(typeof(TSource), typeof(TDestination));

//        Check cache first
//        if (expressionMappings.TryGetValue(key, out var cachedDelegate))
//        {
//            var cachedFunc = (Func<Expression<Func<TSource, bool>>, Expression<Func<TDestination, bool>>>)cachedDelegate;
//            return cachedFunc(predicate);
//        }

//        If not cached, compile and store
//       var compiledFunc = CompileExpressionMapping<TSource, TDestination>();
//        expressionMappings[key] = compiledFunc;

//        return compiledFunc(predicate);
//    }

//    public static Expression<Func<TDestination, object>> MapIncludeExpression<TSource, TDestination>(
//        Expression<Func<TSource, object>> includePath)
//    {
//        ArgumentNullException.ThrowIfNull(includePath);

//        var parameter = Expression.Parameter(typeof(TDestination), "x");
//        var visitor = new ExpressionMappingVisitor<TSource, TDestination>(parameter);
//        var body = visitor.Visit(includePath.Body);

//        return Expression.Lambda<Func<TDestination, object>>(body, parameter);
//    }

//    public static IQueryable<TDestination> MapQueryable<TSource, TDestination>(
//    this IQueryable<TSource> query)
//    {
//        Get the base mapping expression
//        var mapFunc = ExtensoMapper.Map<TSource, TDestination>;
//        var parameter = Expression.Parameter(typeof(TSource), "x");
//        var body = Expression.Invoke(Expression.Constant(mapFunc), parameter);
//        var selector = Expression.Lambda<Func<TSource, TDestination>>(body, parameter);

//        return query.Select(selector);
//    }

//    //Alternative version that accepts an existing mapping expression
//    public static IQueryable<TDestination> MapQueryable<TSource, TDestination>(
//        this IQueryable<TSource> query,
//        Expression<Func<TSource, TDestination>> mappingExpression)
//    {
//        return query.Select(mappingExpression);
//    }

//    public static Expression<Func<TDestination, TDestination>> MapUpdateExpression<TSource, TDestination>(
//    Expression<Func<TSource, TSource>> updateFactory)
//    {
//        ArgumentNullException.ThrowIfNull(updateFactory);

//        var parameter = Expression.Parameter(typeof(TDestination), "x");
//        var visitor = new ExpressionMappingVisitor<TSource, TDestination>(parameter);

//        Visit the entire expression(including the lambda body)
//        var body = (MemberInitExpression)visitor.Visit(updateFactory.Body);

//        return Expression.Lambda<Func<TDestination, TDestination>>(body, parameter);
//    }

//    private static Func<Expression<Func<TSource, bool>>, Expression<Func<TDestination, bool>>> CompileExpressionMapping<TSource, TDestination>() =>
//        predicate =>
//        {
//            var parameter = Expression.Parameter(typeof(TDestination), "x");
//            var visitor = new ExpressionMappingVisitor<TSource, TDestination>(parameter);
//            var body = visitor.Visit(predicate.Body);
//            return Expression.Lambda<Func<TDestination, bool>>(body, parameter);
//        };

//    private class ExpressionMappingVisitor<TModel, TEntity> : ExpressionVisitor
//    {
//        private static readonly ConcurrentDictionary<MemberInfo, MemberInfo> _memberMappingsCache = new();
//        private static readonly ConcurrentDictionary<Type, Type> _typeMappingsCache = new();
//        private readonly ParameterExpression _parameter;

//        public ExpressionMappingVisitor(ParameterExpression parameter)
//        {
//            _parameter = parameter;
//        }

//        protected override Expression VisitLambda<T>(Expression<T> node)
//        {
//            var parameters = node.Parameters.Select(p =>
//                p.Type == typeof(TModel) ? _parameter : p).ToArray();
//            var body = Visit(node.Body);
//            return Expression.Lambda(body, parameters);
//        }

//        protected override Expression VisitMember(MemberExpression node)
//        {
//            Handle primitive types directly
//            if (node.Member.DeclaringType.IsPrimitive ||
//                node.Member.DeclaringType == typeof(string))
//            {
//                return base.VisitMember(node);
//            }

//            Handle nested properties
//            if (node.Member.DeclaringType == typeof(TModel) ||
//                typeof(TModel).IsAssignableFrom(node.Member.DeclaringType))
//            {
//                var newExpression = Visit(node.Expression);
//                var destMember = GetMappedMember(node.Member, newExpression.Type);

//                if (destMember == null)
//                {
//                    throw new InvalidOperationException(
//                        $"No mapping found for member {node.Member.DeclaringType.Name}.{node.Member.Name}");
//                }

//                return Expression.MakeMemberAccess(newExpression, destMember);
//            }

//            return base.VisitMember(node);
//        }

//        protected override Expression VisitMemberInit(MemberInitExpression node)
//        {
//            Handle object initialization expressions like: x => new Customer { Name = x.Name }
//            var newExpression = (NewExpression)Visit(node.NewExpression);

//            var bindings = node.Bindings.Select(binding =>
//            {
//                if (binding is MemberAssignment assignment)
//                {
//                    return Expression.Bind(
//                        GetMappedMember(binding.Member) ?? binding.Member,
//                        Visit(assignment.Expression)
//                    );
//                }
//                return binding;
//            });

//            return Expression.MemberInit(newExpression, bindings);
//        }

//        protected override Expression VisitMethodCall(MethodCallExpression node)
//        {
//            if (node.Method.DeclaringType == typeof(Queryable))
//            {
//                var args = node.Arguments.Select(arg =>
//                {
//                    if (arg is UnaryExpression unary && unary.Operand is LambdaExpression lambda)
//                    {
//                        Handle lambda expressions in LINQ methods
//                        var newParams = lambda.Parameters.Select(p =>
//                            Expression.Parameter(
//                                GetMappedType(p.Type) ?? p.Type,
//                                p.Name));

//                        var newBody = Visit(lambda.Body);
//                        return Expression.Quote(
//                            Expression.Lambda(newBody, newParams));
//                    }
//                    return Visit(arg);
//                }).ToArray();

//                Find the same method in Queryable with potentially different generic args
//                var method = node.Method.GetGenericMethodDefinition();
//                var genericArgs = node.Method.GetGenericArguments()
//                    .Select(t => GetMappedType(t) ?? t)
//                    .ToArray();

//                var newMethod = method.MakeGenericMethod(genericArgs);
//                return Expression.Call(null, newMethod, args);
//            }

//            return base.VisitMethodCall(node);
//        }

//        protected override Expression VisitNew(NewExpression node)
//        {
//            Handle constructor calls
//           var mappedType = GetMappedType(node.Type) ?? node.Type;
//            var args = node.Arguments.Select(Visit);

//            if (node.Members != null)
//            {
//                return Expression.New(
//                    GetMatchingConstructor(mappedType, node.Constructor),
//                    args,
//                    node.Members.Select(m => GetMappedMember(m) ?? m)
//                );
//            }

//            return Expression.New(
//                GetMatchingConstructor(mappedType, node.Constructor),
//                args
//            );
//        }

//        protected override Expression VisitParameter(ParameterExpression node)
//        {
//            return node.Type == typeof(TModel) ? _parameter : base.VisitParameter(node);
//        }

//        private MemberInfo GetMappedMember(MemberInfo sourceMember, Type currentDestType)
//        {
//            Try to find exact match first
//            var destMember = currentDestType.GetProperty(sourceMember.Name);
//            if (destMember != null)
//            {
//                return destMember;
//            }

//            Check if we have a type mapping for the declaring type

//           var sourceDeclaringType = sourceMember.DeclaringType;
//            var mappedType = GetMappedType(sourceDeclaringType);
//            if (mappedType != null)
//                {
//                    return mappedType.GetProperty(sourceMember.Name);
//                }

//            Fall back to source member if no mapping found
//            return sourceMember;
//        }

//        private MemberInfo GetMappedMember(MemberInfo sourceMember)
//        {
//            Don't try to map primitive types
//            if (sourceMember.DeclaringType.IsPrimitive ||
//                sourceMember.DeclaringType == typeof(string) ||
//                sourceMember.DeclaringType == typeof(decimal))
//            {
//                return sourceMember;
//            }

//            return _memberMappingsCache.GetOrAdd(sourceMember, key =>
//            {
//            if (key is PropertyInfo sourceProp)
//            {
//                First try direct property match
//               var destProp = typeof(TEntity).GetProperty(sourceProp.Name);
//                if (destProp != null) return destProp;

//                Handle nested objects
//               var sourceDeclaringType = sourceProp.DeclaringType;
//                var mappedType = GetMappedType(sourceDeclaringType);
//                if (mappedType != null)
//                {
//                    return mappedType.GetProperty(sourceProp.Name) ?? sourceProp;
//                }
//                }
//                return sourceMember; // Return original if no mapping found
//            });
//        }

//        private Type GetMappedType(Type sourceType)
//        {
//            return _typeMappingsCache.GetOrAdd(sourceType, key =>
//            {
//                foreach (var mapping in mappings)
//                {
//                    if (mapping.Key.Source == sourceType)
//                        return mapping.Key.Destination;
//                }
//                return null;
//            });
//        }

//        private ConstructorInfo GetMatchingConstructor(Type type, ConstructorInfo sourceConstructor)
//        {
//            if (sourceConstructor == null) return null;

//            Find constructor with matching parameter types
//            return type.GetConstructors()
//                .FirstOrDefault(c =>
//                    c.GetParameters().Select(p => p.ParameterType)
//                     .SequenceEqual(sourceConstructor.GetParameters()
//                        .Select(p => GetMappedType(p.ParameterType) ?? p.ParameterType)));
//        }
//    }
//}

#endregion Work in Progress
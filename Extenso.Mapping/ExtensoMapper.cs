using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Extenso.Mapping;

public static class ExtensoMapper
{
    private static readonly ConcurrentDictionary<TypePair, Lazy<Delegate>> _expressionMappingCache = new();
    private static readonly ConcurrentDictionary<TypePair, Func<object, object>> mappings = new();

    public static void Register<TSource, TDestination>(Func<TSource, TDestination> mapFunc)
    {
        var key = new TypePair(typeof(TSource), typeof(TDestination));
        mappings[key] = source => mapFunc((TSource)source);
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

    public static Expression<Func<TDestination, bool>> MapPredicate<TSource, TDestination>(
        Expression<Func<TSource, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var key = new TypePair(typeof(TSource), typeof(TDestination));

        var lazyDelegate = _expressionMappingCache.GetOrAdd(key, k =>
            new Lazy<Delegate>(() => CompileExpressionMapping<TSource, TDestination>()));

        var compiledFunc = (Func<Expression<Func<TSource, bool>>, Expression<Func<TDestination, bool>>>)lazyDelegate.Value;
        return compiledFunc(predicate);
    }

    public static Expression<Func<TDestination, TProperty>> MapInclude<TSource, TDestination, TProperty>(
        Expression<Func<TSource, TProperty>> includePath)
    {
        ArgumentNullException.ThrowIfNull(includePath);

        var parameter = Expression.Parameter(typeof(TDestination), "x");
        var visitor = new ExpressionMappingVisitor<TSource, TDestination>(parameter);
        var body = visitor.Visit(includePath.Body);

        return Expression.Lambda<Func<TDestination, TProperty>>(body, parameter);
    }

    public static IQueryable<TDestination> MapQuery<TSource, TDestination>(
        this IQueryable<TSource> query)
    {
        var parameter = Expression.Parameter(typeof(TSource), "x");
        var body = CreateMapExpression<TSource, TDestination>(parameter);
        var selector = Expression.Lambda<Func<TSource, TDestination>>(body, parameter);
        return query.Select(selector);
    }

    public static Expression<Func<TDestination, TDestination>> MapUpdateExpression<TSource, TDestination>(
        Expression<Func<TSource, TSource>> updateFactory)
    {
        ArgumentNullException.ThrowIfNull(updateFactory);

        var parameter = Expression.Parameter(typeof(TDestination), "x");
        var visitor = new ExpressionMappingVisitor<TSource, TDestination>(parameter);

        var body = (MemberInitExpression)visitor.Visit(updateFactory.Body);

        return Expression.Lambda<Func<TDestination, TDestination>>(body, parameter);
    }

    private static Expression CreateMapExpression<TSource, TDestination>(ParameterExpression parameter)
    {
        try
        {
            var bindings = new List<MemberBinding>();
            var destinationType = typeof(TDestination);

            foreach (var destProp in destinationType.GetProperties().Where(p => p.CanWrite))
            {
                var sourceProp = typeof(TSource).GetProperty(destProp.Name);
                if (sourceProp == null) continue;

                var sourceExpr = Expression.Property(parameter, sourceProp);

                // Handle nested properties
                if (!sourceProp.PropertyType.IsPrimitive &&
                    sourceProp.PropertyType != typeof(string) &&
                    sourceProp.PropertyType != destProp.PropertyType)
                {
                    var nestedParameter = Expression.Parameter(sourceProp.PropertyType, "nested");
                    var nestedMap = CreateMapExpression(nestedParameter, sourceProp.PropertyType, destProp.PropertyType);
                    var nestedLambda = Expression.Lambda(nestedMap, nestedParameter);
                    var mappedExpr = Expression.Invoke(nestedLambda, sourceExpr);
                    bindings.Add(Expression.Bind(destProp, mappedExpr));
                }
                else
                {
                    bindings.Add(Expression.Bind(destProp, sourceExpr));
                }
            }

            return Expression.MemberInit(Expression.New(destinationType), bindings);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create mapping expression from {typeof(TSource)} to {typeof(TDestination)}", ex);
        }
    }

    private static Expression CreateMapExpression(ParameterExpression parameter, Type sourceType, Type destType)
    {
        var bindings = new List<MemberBinding>();

        foreach (var destProp in destType.GetProperties().Where(p => p.CanWrite))
        {
            var sourceProp = sourceType.GetProperty(destProp.Name);
            if (sourceProp == null) continue;

            var sourceExpr = Expression.Property(parameter, sourceProp);

            if (!sourceProp.PropertyType.IsPrimitive &&
                sourceProp.PropertyType != typeof(string) &&
                sourceProp.PropertyType != destProp.PropertyType)
            {
                var nestedParameter = Expression.Parameter(sourceProp.PropertyType, "nested");
                var nestedMap = CreateMapExpression(nestedParameter, sourceProp.PropertyType, destProp.PropertyType);
                var nestedLambda = Expression.Lambda(nestedMap, nestedParameter);
                bindings.Add(Expression.Bind(destProp, Expression.Invoke(nestedLambda, sourceExpr)));
            }
            else
            {
                bindings.Add(Expression.Bind(destProp, sourceExpr));
            }
        }

        return Expression.MemberInit(Expression.New(destType), bindings);
    }

    //private static Expression BuildMapExpression<TSource, TDestination>(ParameterExpression parameter)
    //{
    //    var visitor = new ExpressionMappingVisitor<TSource, TDestination>(parameter);
    //    var destinationType = typeof(TDestination);

    //    var bindings = destinationType.GetProperties()
    //        .Where(p => p.CanWrite)
    //        .Select(destProp =>
    //        {
    //            var sourceProp = typeof(TSource).GetProperty(destProp.Name);
    //            if (sourceProp == null) return null;

    //            var sourcePropAccess = Expression.Property(parameter, sourceProp);

    //            if (sourceProp.PropertyType != destProp.PropertyType &&
    //                !sourceProp.PropertyType.IsValueType &&
    //                !destProp.PropertyType.IsValueType)
    //            {
    //                return Expression.Bind(
    //                    destProp,
    //                    BuildMapExpression(sourceProp.PropertyType, destProp.PropertyType)
    //                );
    //            }

    //            return Expression.Bind(destProp, sourcePropAccess);
    //        })
    //        .Where(b => b != null)
    //        .ToList();

    //    return Expression.MemberInit(
    //        Expression.New(typeof(TDestination)),
    //        bindings
    //    );
    //}

    //private static Expression BuildMapExpression(Type sourceType, Type destType)
    //{
    //    var parameter = Expression.Parameter(sourceType, "x");
    //    var visitor = new ExpressionMappingVisitor<object, object>(parameter);

    //    var destProperties = destType.GetProperties().Where(p => p.CanWrite);
    //    var bindings = new List<MemberBinding>();

    //    foreach (var destProp in destProperties)
    //    {
    //        var sourceProp = sourceType.GetProperty(destProp.Name);
    //        if (sourceProp == null) continue;

    //        var sourcePropAccess = Expression.Property(parameter, sourceProp);
    //        bindings.Add(Expression.Bind(destProp, sourcePropAccess));
    //    }

    //    return Expression.Lambda(
    //        Expression.MemberInit(Expression.New(destType), bindings),
    //        parameter
    //    ).Body;
    //}

    private static Func<Expression<Func<TSource, bool>>, Expression<Func<TDestination, bool>>> CompileExpressionMapping<TSource, TDestination>() =>
        predicate =>
        {
            var parameter = Expression.Parameter(typeof(TDestination), "x");
            var visitor = new ExpressionMappingVisitor<TSource, TDestination>(parameter);
            var body = visitor.Visit(predicate.Body);
            return Expression.Lambda<Func<TDestination, bool>>(body, parameter);
        };

    private class ExpressionMappingVisitor<TModel, TEntity> : ExpressionVisitor
    {
        private static readonly ConcurrentDictionary<MemberInfo, MemberInfo> _memberMappingsCache = new();
        private static readonly ConcurrentDictionary<Type, Type> _typeMappingsCache = new();
        private readonly ParameterExpression _parameter;
        private readonly Dictionary<MemberExpression, MemberExpression> _visitedMembers = [];

        public ExpressionMappingVisitor(ParameterExpression parameter)
        {
            _parameter = parameter;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var parameters = node.Parameters.Select(p =>
                p.Type == typeof(TModel) ? _parameter : p).ToArray();
            var body = Visit(node.Body);
            return Expression.Lambda(body, parameters);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            // Check cache first
            if (_visitedMembers.TryGetValue(node, out var cachedResult))
            {
                return cachedResult;
            }

            // Handle primitive types directly
            if (node.Member.DeclaringType?.IsPrimitive == true ||
                node.Member.DeclaringType == typeof(string) ||
                node.Member.DeclaringType == typeof(decimal))
            {
                return base.VisitMember(node);
            }

            var newExpression = Visit(node.Expression);
            var destMember = GetMappedMember(node.Member, newExpression?.Type) ?? node.Member;

            var result = Expression.MakeMemberAccess(newExpression, destMember);

            // Cache the result
            _visitedMembers[node] = result;

            return result;
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            var newExpression = (NewExpression)Visit(node.NewExpression);

            var bindings = node.Bindings.Select(binding =>
            {
                if (binding is MemberAssignment assignment)
                {
                    return Expression.Bind(
                        GetMappedMember(binding.Member) ?? binding.Member,
                        Visit(assignment.Expression)
                    );
                }
                return binding;
            });

            return Expression.MemberInit(newExpression, bindings);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable))
            {
                var args = node.Arguments.Select(arg =>
                {
                    if (arg is UnaryExpression unary && unary.Operand is LambdaExpression lambda)
                    {
                        var newParams = lambda.Parameters.Select(p =>
                            Expression.Parameter(
                                GetMappedType(p.Type) ?? p.Type,
                                p.Name));

                        var newBody = Visit(lambda.Body);
                        return Expression.Quote(
                            Expression.Lambda(newBody, newParams));
                    }
                    return Visit(arg);
                }).ToArray();

                var method = node.Method.GetGenericMethodDefinition();
                var genericArgs = node.Method.GetGenericArguments()
                    .Select(t => GetMappedType(t) ?? t)
                    .ToArray();

                var newMethod = method.MakeGenericMethod(genericArgs);
                return Expression.Call(null, newMethod, args);
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitNew(NewExpression node)
        {
            var mappedType = GetMappedType(node.Type) ?? node.Type;
            var args = node.Arguments.Select(Visit);

            if (node.Members != null)
            {
                return Expression.New(
                    GetMatchingConstructor(mappedType, node.Constructor),
                    args,
                    node.Members.Select(m => GetMappedMember(m) ?? m)
                );
            }

            return Expression.New(
                GetMatchingConstructor(mappedType, node.Constructor),
                args
            );
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node.Type == typeof(TModel) ? _parameter : base.VisitParameter(node);
        }

        private static MemberInfo GetMappedMember(MemberInfo sourceMember, Type currentDestType)
        {
            // First try exact match in current type
            var destMember = currentDestType?.GetProperty(sourceMember.Name,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (destMember != null) return destMember;

            // Check if declaring type is mapped
            var sourceDeclaringType = sourceMember.DeclaringType;
            if (sourceDeclaringType == null) return null;

            var mappedType = GetMappedType(sourceDeclaringType);
            if (mappedType != null)
            {
                return mappedType.GetProperty(sourceMember.Name,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            }

            return null;
        }

        private static MemberInfo GetMappedMember(MemberInfo sourceMember)
        {
            if (sourceMember.DeclaringType.IsPrimitive ||
                sourceMember.DeclaringType == typeof(string) ||
                sourceMember.DeclaringType == typeof(decimal))
            {
                return sourceMember;
            }

            return _memberMappingsCache.GetOrAdd(sourceMember, key =>
            {
                if (key is PropertyInfo sourceProp)
                {
                    var destProp = typeof(TEntity).GetProperty(sourceProp.Name);
                    if (destProp != null) return destProp;

                    var sourceDeclaringType = sourceProp.DeclaringType;
                    var mappedType = GetMappedType(sourceDeclaringType);
                    if (mappedType != null)
                    {
                        return mappedType.GetProperty(sourceProp.Name) ?? sourceProp;
                    }
                }
                return sourceMember; // Return original if no mapping found
            });
        }

        private static Type GetMappedType(Type sourceType)
        {
            if (sourceType == null) return null;

            // Check cache first
            if (_typeMappingsCache.TryGetValue(sourceType, out var cachedType))
            {
                return cachedType;
            }

            // Optimized lookup in mappings dictionary
            foreach (var mapping in mappings)
            {
                if (mapping.Key.Source == sourceType)
                {
                    _typeMappingsCache[sourceType] = mapping.Key.Destination;
                    return mapping.Key.Destination;
                }
            }

            _typeMappingsCache[sourceType] = null; // Cache negative results too
            return null;
        }

        private static ConstructorInfo GetMatchingConstructor(Type type, ConstructorInfo sourceConstructor) => sourceConstructor == null
            ? null
            : type.GetConstructors()
            .FirstOrDefault(c =>
                c.GetParameters().Select(p => p.ParameterType)
                    .SequenceEqual(sourceConstructor.GetParameters()
                    .Select(p => GetMappedType(p.ParameterType) ?? p.ParameterType)));
    }

    private class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            if (node == _oldValue) return _newValue;
            return base.Visit(node);
        }
    }
}
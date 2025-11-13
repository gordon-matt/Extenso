using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Extenso.Mapping;

public static class ExpressionTypeMapper
{
    public static Expression ReplaceTypes(Expression expression, IDictionary<Type, Type> replacements)
    {
        var newExpression = new TypeReplacementVisitor(replacements).Visit(expression);
        return newExpression;
    }

    private class TypeReplacementVisitor : ExpressionVisitor
    {
        private readonly IDictionary<Type, Type?> _typeReplacementCache = new Dictionary<Type, Type?>();
        private readonly Stack<Dictionary<ParameterExpression, ParameterExpression>> _currentParameters = new();

        public TypeReplacementVisitor(IDictionary<Type, Type> typeReplacement)
        {
            foreach (var r in typeReplacement)
            {
                _typeReplacementCache.Add(r.Key, r.Value);
            }
        }

        private bool TryMapType(Type type, [NotNullWhen(true)] out Type? replacement)
        {
            if (_typeReplacementCache.TryGetValue(type, out replacement))
                return replacement != null;

            if (type.IsGenericType)
            {
                if (type.GetGenericArguments().Any(NeedsMapping))
                {
                    var types = type.GetGenericArguments().Select(MapType).ToArray();
                    replacement = type.GetGenericTypeDefinition().MakeGenericType(types);
                }
            }

            _typeReplacementCache.Add(type, replacement);

            return replacement != null;
        }

        private void RegisterReplacement(Type oldType, Type newType)
        {
            if (oldType.IsGenericType)
            {
                if (!newType.IsGenericType)
                    throw new InvalidOperationException($"Cannot replace generic type '{oldType.FullName}' with non-generic type '{newType.FullName}'.");

                var oldGenericArgs = oldType.GetGenericArguments();
                var newGenericArgs = newType.GetGenericArguments();

                if (oldGenericArgs.Length != newGenericArgs.Length)
                    throw new InvalidOperationException($"Cannot replace generic type '{oldType.FullName}' with '{newType.FullName}' due to different number of generic arguments.");

                for (int i = 0; i < oldGenericArgs.Length; i++)
                {
                    RegisterReplacement(oldGenericArgs[i], newGenericArgs[i]);
                }
            }
            else
            {
                _typeReplacementCache[oldType] = newType;
            }
        }

        private MemberInfo ReplaceMember(MemberInfo memberInfo, Type targetType)
        {
            var newMembers = targetType.GetMember(memberInfo.Name);
            return newMembers.Length == 0
                ? throw new InvalidOperationException($"There is no member '{memberInfo.Name}' in type '{targetType.FullName}'")
                : newMembers.Length > 1
                ? throw new InvalidOperationException($"Ambiguous member '{memberInfo.Name}' in type '{targetType.FullName}'")
                : newMembers[0];
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var replacements = node.Parameters.ToDictionary(p => p,
                p => TryMapType(p.Type, out var replacementType)
                    ? Expression.Parameter(replacementType, p.Name)
                    : p);

            _currentParameters.Push(replacements);

            var newBody = Visit(node.Body);

            _currentParameters.Pop();

            if (ReferenceEquals(newBody, node.Body) && replacements.All(pair => pair.Key != pair.Value))
            {
                // nothing changed
                return node;
            }

            var newParameters = replacements.Select(pair => pair.Value);
            var lambdaType = MapType(node.Type);
            return Expression.Lambda(lambdaType, newBody, node.Name, node.TailCall, newParameters);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            foreach (var dict in _currentParameters)
            {
                if (dict.TryGetValue(node, out var newNode))
                    return newNode;
            }

            return base.VisitParameter(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null && TryMapType(node.Expression.Type, out var replacement))
            {
                var expr = Visit(node.Expression);
                if (expr.Type != replacement)
                    throw new InvalidOperationException($"Invalid replacement of '{node.Expression}' to type '{replacement.FullName}'.");

                var prop = replacement.GetProperty(node.Member.Name);
                if (prop == null)
                    throw new InvalidOperationException($"Property not found in target type: {replacement.FullName}.{node.Member.Name}");

                RegisterReplacement(node.Type, prop.PropertyType);

                return Expression.MakeMemberAccess(expr, prop);
            }

            return base.VisitMember(node);
        }

        protected override Expression VisitNew(NewExpression node)
        {
            if (TryMapType(node.Type, out var replacement) && node.Constructor != null)
            {
                var paramTypes = node.Constructor.GetParameters()
                    .Select(p => p.ParameterType)
                    .ToArray();

                var ctor = replacement.GetConstructor(paramTypes);

                if (ctor == null)
                {
                    string name = replacement.FullName + "." + node.Constructor.Name + "(" +
                                string.Join(", ", paramTypes.Select(t => t.Name)) + ")";
                    throw new InvalidOperationException($"Constructor not found in target type: {name}");
                }

                var newArguments = node.Arguments.Select(Visit);
                if (node.Members != null)
                {
                    var newMembers = node.Members.Select(m => ReplaceMember(m, replacement));
                    var newExpression = Expression.New(ctor, newArguments!, newMembers);
                    return newExpression;
                }
                else
                {
                    var newExpression = Expression.New(ctor, newArguments!);
                    return newExpression;
                }
            }

            return base.VisitNew(node);
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            if (TryMapType(node.Type, out var replacement))
            {
                var newExpression = (NewExpression)Visit(node.NewExpression);
                var newBindings = node.Bindings.Select(b =>
                {
                    switch (b.BindingType)
                    {
                        case MemberBindingType.Assignment:
                            {
                                var mab = (MemberAssignment)b;
                                return Expression.Bind(ReplaceMember(mab.Member, replacement),
                                    Visit(mab.Expression));
                            }
                        case MemberBindingType.MemberBinding:
                            {
                                throw new NotImplementedException();
                            }
                        case MemberBindingType.ListBinding:
                            {
                                throw new NotImplementedException();
                            }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                });

                var newMemberInit = Expression.MemberInit(newExpression, newBindings);
                return newMemberInit;
            }

            return base.VisitMemberInit(node);
        }

        private Type MapType(Type type) => TryMapType(type, out var newType) ? newType : type;

        private bool NeedsMapping(Type type) => TryMapType(type, out _);

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType != null)
            {
                var newDeclaringType = MapType(node.Method.DeclaringType);

                if (newDeclaringType != node.Method.DeclaringType ||
                    (node.Object != null && NeedsMapping(node.Object.Type)) ||
                    node.Method.GetParameters().Any(p => NeedsMapping(p.ParameterType)) ||
                    (node.Method.IsGenericMethod && node.Method.GetGenericArguments().Any(NeedsMapping)))
                {
                    var newObject = Visit(node.Object);
                    var newArguments = node.Arguments.Select(Visit).ToArray();

                    var newGenericArguments = Type.EmptyTypes;
                    if (node.Method.IsGenericMethod)
                        newGenericArguments = node.Method.GetGenericArguments().Select(MapType).ToArray();

                    if (newObject != null)
                    {
                        var newCall = Expression.Call(newObject!, node.Method.Name, newGenericArguments, newArguments!);
                        return newCall;
                    }
                    else
                    {
                        var newCall = Expression.Call(newDeclaringType, node.Method.Name, newGenericArguments, newArguments!);
                        return newCall;
                    }
                }
            }

            return base.VisitMethodCall(node);
        }
    }
}
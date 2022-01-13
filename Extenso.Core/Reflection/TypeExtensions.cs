﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Extenso.Reflection
{
    /// <summary>
    /// Provides a set of static methods for querying types.
    /// </summary>
    public static class TypeExtensions
    {
        //private static readonly Lazy<Type[]> simpleTypes;

        //static TypeExtensions()
        //{
        //    simpleTypes = new Lazy<Type[]>(() =>
        //    {
        //        var types = new[]
        //        {
        //            typeof(Boolean),
        //            typeof(Byte),
        //            typeof(Char),
        //            typeof(DateTime),
        //            typeof(DateTimeOffset),
        //            typeof(Decimal),
        //            typeof(Double),
        //            typeof(Enum),
        //            typeof(Guid),
        //            typeof(Int16),
        //            typeof(Int32),
        //            typeof(Int64),
        //            typeof(IntPtr),
        //            typeof(SByte),
        //            typeof(Single),
        //            typeof(String),
        //            typeof(TimeSpan),
        //            typeof(UInt16),
        //            typeof(UInt32),
        //            typeof(UInt64),
        //            typeof(UIntPtr),
        //            typeof(Uri)
        //        };

        //        var nullTypes = types
        //            .Where(t => t.GetTypeInfo().IsValueType)
        //            .Select(t => typeof(Nullable<>).MakeGenericType(t));

        //        return types.Concat(nullTypes).ToArray();
        //    });
        //}

        /// <summary>
        /// Returns the default value for the given type.
        /// </summary>
        /// <param name="type">The type of which to return the default value for.</param>
        /// <returns>The default value of the given type, if it is a value type; otherwise false.</returns>
        public static object GetDefaultValue(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        /// <summary>
        /// Returns a collection of System.Reflection.MethodInfo for all extension methods for the given type in the specified assembly.
        /// </summary>
        /// <param name="type">The type to find extension methods for.</param>
        /// <param name="extensionsAssembly">The System.Reflection.Assembly in which to search for extension methods for the given type.</param>
        /// <returns>A collection of System.Reflection.MethodInfo for all extension methods for type in extensionsAssembly.</returns>
        public static IEnumerable<MethodInfo> GetExtensionMethods(this Type type, Assembly extensionsAssembly)
        {
            return from t in extensionsAssembly.GetTypes()
                   where !t.GetTypeInfo().IsGenericType && !t.IsNested
                   from m in t.GetTypeInfo().GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                   where m.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false)
                   where m.GetParameters()[0].ParameterType == type
                   select m;
        }

        /// <summary>
        /// A System.Reflection.MethodInfo for the specified extension method for the given type in the specified assembly.
        /// </summary>
        /// <param name="type">The type to find the extension method for.</param>
        /// <param name="extensionsAssembly">The System.Reflection.Assembly in which to search for the extension method for the given type.</param>
        /// <param name="name">The name of the specific extension method to find.</param>
        /// <returns>A System.Reflection.MethodInfo for the specified extension method, if found; otherwise null.</returns>
        public static MethodInfo GetExtensionMethod(this Type type, Assembly extensionsAssembly, string name)
        {
            return type.GetExtensionMethods(extensionsAssembly).FirstOrDefault(m => m.Name == name);
        }

        /// <summary>
        /// A System.Reflection.MethodInfo for the specified extension method for the given type in the specified
        /// assembly that has parameters whose types match those of parameterTypes.
        /// </summary>
        /// <param name="type">The type to find the extension method for.</param>
        /// <param name="extensionsAssembly">The System.Reflection.Assembly in which to search for the extension method for the given type.</param>
        /// <param name="name">The name of the specific extension method to find.</param>
        /// <param name="parameterTypes">
        /// An argument list for the extension method. These must be in the same order and of the same types as the paremeters of the
        /// extension method being searched for.
        /// </param>
        /// <returns>A System.Reflection.MethodInfo for the specified extension method, if found; otherwise null.</returns>
        public static MethodInfo GetExtensionMethod(this Type type, Assembly extensionsAssembly, string name, Type[] parameterTypes)
        {
            var methods = (from m in type.GetExtensionMethods(extensionsAssembly)
                           where m.Name == name
                           && m.GetParameters().Count() == parameterTypes.Length + 1 // + 1 because extension method parameter (this)
                           select m).ToList();

            if (!methods.Any())
            {
                return default(MethodInfo);
            }

            if (methods.Count() == 1)
            {
                return methods.First();
            }

            foreach (var methodInfo in methods)
            {
                var parameters = methodInfo.GetParameters();

                bool found = true;
                for (byte b = 0; b < parameterTypes.Length; b++)
                {
                    found = true;
                    if (parameters[b].GetType() != parameterTypes[b])
                    {
                        found = false;
                    }
                }

                if (found)
                {
                    return methodInfo;
                }
            }

            return default(MethodInfo);
        }

        /// <summary>
        /// Gets a value indicating whether type is a collection.
        /// </summary>
        /// <param name="type">The type to examine.</param>
        /// <returns>true if type is a collection; otherwise false.</returns>
        public static bool IsCollection(this Type type)
        {
            // string implements IEnumerable (it's a collection of System.Char), but for our purposes we don't consider it a collection.
            if (type == typeof(string))
            {
                return false;
            }

            var interfaces = from @interface in type.GetTypeInfo().GetInterfaces()
                             where @interface == typeof(IEnumerable) ||
                                 (@interface.GetTypeInfo().IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                             select @interface;
            return interfaces.Count() != 0;
        }

        /// <summary>
        /// Gets a value indicating whether type is a generic collection.
        /// </summary>
        /// <param name="type">The type to examine.</param>
        /// <returns>true if type is a generic collection; otherwise false.</returns>
        public static bool IsGenericCollection(this Type type)
        {
            var collectionType = typeof(ICollection<>);
            var typeInfo = type.GetTypeInfo();
            var collectionTypeInfo = collectionType.GetTypeInfo();

            if (typeInfo.IsGenericType && collectionTypeInfo.IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                return true;
            }

            var interfaces = typeInfo.GetInterfaces();
            return interfaces.Any(@interface => @interface.GetTypeInfo().IsGenericType && collectionTypeInfo.IsAssignableFrom(@interface.GetGenericTypeDefinition()));
        }

        /// <summary>
        /// Gets a value indicating whether type is nullable
        /// </summary>
        /// <param name="type">The type to examine.</param>
        /// <returns>true if type is nullable; otherwise false.</returns>
        public static bool IsNullable(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            var typeInfo = type.GetTypeInfo();
            if (!typeInfo.IsValueType)
            {
                return true;
            }

            return typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

            //return Nullable.GetUnderlyingType(type) != null; //faster than above version? Needs testing
        }

        /// <summary>
        /// Gets a value indicating whether type is numeric (int, long, byte, float, double, etc).
        /// </summary>
        /// <param name="type">The type to examine.</param>
        /// <param name="includeNullable">if true, will consider type as numeric even if it is nullable (int?, long?, byte?, float?, double?, etc).</param>
        /// <returns>true if type is numeric; otherwise false.</returns>
        public static bool IsNumeric(this Type type, bool includeNullable = true)
        {
            if (type.IsNullable())
            {
                if (includeNullable)
                {
                    type = Nullable.GetUnderlyingType(type);
                }
                else
                {
                    return false;
                }
            }

            if (type.In(typeof(byte),
                typeof(decimal),
                typeof(double),
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(sbyte),
                typeof(float),
                typeof(ushort),
                typeof(uint),
                typeof(ulong)))
            {
                return true;
            }

            return false;
        }

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="type">The type to examine.</param>
        ///// <returns></returns>
        //public static bool IsSimple(this Type type)
        //{
        //    if (type.GetTypeInfo().IsPrimitive || simpleTypes.Value.Any(x => x.GetTypeInfo().IsAssignableFrom(type)))
        //    {
        //        return true;
        //    }

        //    var nut = Nullable.GetUnderlyingType(type);
        //    return nut != null && nut.GetTypeInfo().IsEnum;
        //}

        /// <summary>
        /// Converts a value type to its nullable equivalent.
        /// </summary>
        /// <param name="type">The type to convert.</param>
        /// <returns>The nullable equivalent of type.</returns>
        public static Type ToNullable(this Type type)
        {
            if (type == null)
            {
                return null;
            }
            if (type.IsNullable())
            {
                return type;
            }
            if (type.GetTypeInfo().IsValueType && type != typeof(void))
            {
                return typeof(Nullable<>).MakeGenericType(type);
            }
            return null;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Extenso.Reflection
{
    /// <summary>
    /// Provides a set of static methods for manipulating objects and providing access to object metadata.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns the value of the private field with the given name for obj.
        /// </summary>
        /// <typeparam name="T">The type of obj.</typeparam>
        /// <param name="obj">The object to get the private field value from.</param>
        /// <param name="fieldName">The name of the private field to get the value for.</param>
        /// <returns>The value of the specified private field for obj.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static object GetPrivateFieldValue<T>(this T obj, string fieldName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var type = typeof(T);
            FieldInfo fieldInfo = null;
            while (fieldInfo == null && type != null)
            {
                var typeInfo = type.GetTypeInfo();
                fieldInfo = typeInfo.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                type = typeInfo.BaseType;
            }

            if (fieldInfo == null)
            {
                throw new ArgumentOutOfRangeException(
                    "fieldName",
                    $"Field {fieldName} was not found in Type {typeof(T).FullName}");
            }

            return fieldInfo.GetValue(obj);
        }

        /// <summary>
        /// Returns the value of the private property with the given name for obj.
        /// </summary>
        /// <typeparam name="T">The type of obj.</typeparam>
        /// <param name="obj">The object to get the private property value from.</param>
        /// <param name="propertyName">The name of the private property to get the value for.</param>
        /// <returns>The value of the specified private property for obj.</returns>
        public static object GetPrivatePropertyValue<T>(this T obj, string propertyName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var property = typeof(T).GetTypeInfo().GetProperty(
                propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (property == null)
            {
                throw new ArgumentOutOfRangeException(
                   "propertyName",
                   $"Property {propertyName} was not found in Type {typeof(T).FullName}");
            }

            return property.GetValue(obj, null);
        }

        /// <summary>
        /// Returns the value of the property specified by the given System.Reflection.PropertyInfo for obj.
        /// </summary>
        /// <typeparam name="T">The type of obj.</typeparam>
        /// <param name="obj">The object to get the property value from.</param>
        /// <param name="property">The System.Reflection.PropertyInfo to get the value for.</param>
        /// <returns>The value of the specified property for obj.</returns>
        public static object GetPropertyValue<T>(this T obj, PropertyInfo property)
        {
            return property.GetValue(obj, null);
        }

        /// <summary>
        /// Returns the value of the property with the given name for obj.
        /// </summary>
        /// <typeparam name="T">The type of obj.</typeparam>
        /// <param name="obj">The object to get the property value from.</param>
        /// <param name="propertyName">The name of the property to get the value for.</param>
        /// <returns>The value of the specified property for obj.</returns>
        public static object GetPropertyValue<T>(this T obj, string propertyName)
        {
            var property = obj.GetType().GetTypeInfo().GetProperty(propertyName);
            return GetPropertyValue(obj, property);
        }

        /// <summary>
        /// Gets a value indicating whether obj has a property with the given name.
        /// </summary>
        /// <typeparam name="T">The type of obj.</typeparam>
        /// <param name="obj">The object to examine.</param>
        /// <param name="propertyName">The name of the property to find in obj.</param>
        /// <returns>true if obj has a property with the given name; otherwise false.</returns>
        public static bool HasProperty<T>(this T obj, string propertyName)
        {
            return typeof(T).GetTypeInfo().GetProperties().SingleOrDefault(p => p.Name.Equals(propertyName)) != null;
        }

        /// <summary>
        /// Invokes the extension method with the given name in the given assembly, using the specified parameters.
        /// Use this if your return type is NOT a collection; otherwise use InvokeExtensionMethodForCollection().
        /// </summary>
        /// <typeparam name="T">The type of obj.</typeparam>
        /// <param name="obj">The object upon which to invoke the extension method.</param>
        /// <param name="extensionsAssembly">The System.Reflection.Assembly wherein to find the extension method.</param>
        /// <param name="methodName">The name of the extension method to find in extensionsAssembly.</param>
        /// <param name="parameters">
        /// An argument list for the invoked extension method. This is an array of objects
        /// with the same number, order, and type as the parameters of the extension method
        /// to be invoked. If there are no parameters, parameters should be null. If the
        /// extension method represented by this instance takes a ref parameter (ByRef
        /// in Visual Basic), no special attribute is required for that parameter in order
        /// to invoke the extension method using this function. Any object in this array
        /// that is not explicitly initialized with a value will contain the default value
        /// for that object type. For reference-type elements, this value is null. For value-type
        /// elements, this value is 0, 0.0, or false, depending on the specific element type.
        /// </param>
        /// <returns>An object containing the return value of the invoked extension method.</returns>
        public static object InvokeExtensionMethod<T>(this T obj, Assembly extensionsAssembly, string methodName, params object[] parameters)
        {
            var newParameters = new object[parameters.Length + 1];
            newParameters[0] = obj;

            for (byte b = 0; b < parameters.Length; b++)
            {
                newParameters[b + 1] = parameters[b];
            }

            var types = new Type[parameters.Length];

            for (int i = 0; i < types.Length; i++)
            {
                types[i] = parameters[i].GetType();
            }

            var methodInfo = typeof(T).GetExtensionMethod(extensionsAssembly, methodName, types);
            var value = methodInfo.Invoke(obj, newParameters);

            return value;
        }

        /// <summary>
        /// Invokes the extension method with the given name in the given assembly, using the specified parameters.
        /// Use this if your return type is a collection; otherwise use InvokeExtensionMethod().
        /// </summary>
        /// <typeparam name="T">The type of obj.</typeparam>
        /// <param name="obj">The object upon which to invoke the extension method.</param>
        /// <param name="extensionsAssembly">The System.Reflection.Assembly wherein to find the extension method.</param>
        /// <param name="methodName">The name of the extension method to find in extensionsAssembly.</param>
        /// <param name="parameters">
        /// An argument list for the invoked extension method. This is an array of objects
        /// with the same number, order, and type as the parameters of the extension method
        /// to be invoked. If there are no parameters, parameters should be null. If the
        /// extension method represented by this instance takes a ref parameter (ByRef
        /// in Visual Basic), no special attribute is required for that parameter in order
        /// to invoke the extension method using this function. Any object in this array
        /// that is not explicitly initialized with a value will contain the default value
        /// for that object type. For reference-type elements, this value is null. For value-type
        /// elements, this value is 0, 0.0, or false, depending on the specific element type.
        /// </param>
        /// <returns>A collection of objects containing the return values of the invoked extension method.</returns>
        public static IEnumerable<object> InvokeExtensionMethodForCollection<T>(this T obj, Assembly extensionsAssembly, string methodName, params object[] parameters)
        {
            var newParameters = new object[parameters.Length + 1];
            newParameters[0] = obj;

            for (byte b = 0; b < parameters.Length; b++)
            {
                newParameters[b + 1] = parameters[b];
            }

            var types = new Type[parameters.Length];

            for (int i = 0; i < types.Length; i++)
            {
                types[i] = parameters[i].GetType();
            }

            var methodInfo = typeof(T).GetExtensionMethod(extensionsAssembly, methodName, types);
            var value = methodInfo.Invoke(obj, newParameters);

            if (value == null)
            {
                return Enumerable.Empty<object>();
            }

            return ((IEnumerable)value).OfType<object>();
        }

        /// <summary>
        /// Invokes the method with the given name, using the specified parameters.
        /// </summary>
        /// <typeparam name="T">The type of obj.</typeparam>
        /// <param name="obj">The object upon which to invoke the method.</param>
        /// <param name="methodName">The name of the method to find in obj.</param>
        /// <param name="parameters">
        /// An argument list for the invoked method. This is an array of objects
        /// with the same number, order, and type as the parameters of the method
        /// to be invoked. If there are no parameters, parameters should be null. If the
        /// method represented by this instance takes a ref parameter (ByRef
        /// in Visual Basic), no special attribute is required for that parameter in order
        /// to invoke the method using this function. Any object in this array
        /// that is not explicitly initialized with a value will contain the default value
        /// for that object type. For reference-type elements, this value is null. For value-type
        /// elements, this value is 0, 0.0, or false, depending on the specific element type.
        /// </param>
        /// <returns>An object containing the return value of the invoked method.</returns>
        public static object InvokeMethod<T>(this T obj, string methodName, params object[] parameters)
        {
            var types = new Type[parameters.Length];

            for (int i = 0; i < types.Length; i++)
            {
                types[i] = parameters[i].GetType();
            }

            var methodInfo = typeof(T).GetTypeInfo().GetMethod(methodName, types);
            var value = methodInfo.Invoke(obj, parameters);
            return value;
        }

        /// <summary>
        /// Sets the value of the specified private field on the given object.
        /// </summary>
        /// <typeparam name="T">The type of obj.</typeparam>
        /// <param name="obj">The object whose private field value will be set.</param>
        /// <param name="fieldName">The name of the private field to set the value for.</param>
        /// <param name="value">The value to assign to the private field.</param>
        public static void SetPrivateFieldValue<T>(this T obj, string fieldName, object value)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var type = typeof(T);
            FieldInfo fieldInfo = null;

            while (fieldInfo == null && type != null)
            {
                var typeInfo = type.GetTypeInfo();
                fieldInfo = typeInfo.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                type = typeInfo.BaseType;
            }

            if (fieldInfo == null)
            {
                throw new ArgumentOutOfRangeException(
                   "fieldName",
                   $"Field {fieldName} was not found in Type {obj.GetType().FullName}");
            }

            fieldInfo.SetValue(obj, value);
        }

        /// <summary>
        /// Sets the value of the specified private property on the given object.
        /// </summary>
        /// <typeparam name="T">The type of obj.</typeparam>
        /// <param name="obj">The object whose private property value will be set.</param>
        /// <param name="propertyName">The name of the private property to set the value for.</param>
        /// <param name="value">The value to assign to the private property.</param>
        public static void SetPrivatePropertyValue<T>(this T obj, string propertyName, object value)
        {
            var type = typeof(T);
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.GetProperty(
                propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
            {
                throw new ArgumentOutOfRangeException(
                    "propertyName",
                    $"Property {propertyName} was not found in Type {typeof(T).FullName}");
            }

            type.InvokeMember(
                propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance,
                null, obj,
                new object[] { value });
        }

        /// <summary>
        /// Sets the value of the specified property on the given object.
        /// </summary>
        /// <typeparam name="T">The type of obj.</typeparam>
        /// <param name="obj">The object whose property value will be set.</param>
        /// <param name="propertyName">The name of the property to set the value for.</param>
        /// <param name="value">The value to assign to the property.</param>
        public static void SetPropertyValue<T>(this T obj, string propertyName, object value)
        {
            SetPropertyValue(obj, obj.GetType().GetTypeInfo().GetProperty(propertyName), value);
        }

        /// <summary>
        /// Sets the value of the property specified by the given System.Reflection.PropertyInfo for obj.
        /// </summary>
        /// <typeparam name="T">The type of obj.</typeparam>
        /// <param name="obj">The object whose property value will be set.</param>
        /// <param name="property">The name of the property to set the value for.</param>
        /// <param name="value">The value to assign to the property.</param>
        public static void SetPropertyValue<T>(this T obj, PropertyInfo property, object value)
        {
            var propertyType = property.PropertyType;
            string valueAsString = value.ToString();

            if (propertyType == typeof(String))
            {
                property.SetValue(obj, valueAsString, null);
            }
            else if (propertyType == typeof(Int32))
            {
                property.SetValue(obj, Convert.ToInt32(valueAsString), null);
            }
            else if (propertyType == typeof(Guid))
            {
                property.SetValue(obj, new Guid(valueAsString), null);
            }
            else if (propertyType.GetTypeInfo().IsEnum)
            {
                var enumValue = Enum.Parse(propertyType, valueAsString);
                property.SetValue(obj, enumValue, null);
            }
            else if (propertyType == typeof(Boolean))
            {
                property.SetValue(obj, Convert.ToBoolean(valueAsString), null);
            }
            else if (propertyType == typeof(DateTime))
            {
                property.SetValue(obj, Convert.ToDateTime(valueAsString), null);
            }
            else if (propertyType == typeof(Single))
            {
                property.SetValue(obj, Convert.ToSingle(valueAsString), null);
            }
            else if (propertyType == typeof(Decimal))
            {
                property.SetValue(obj, Convert.ToDecimal(valueAsString), null);
            }
            else if (propertyType == typeof(Byte))
            {
                property.SetValue(obj, Convert.ToByte(valueAsString), null);
            }
            else if (propertyType == typeof(Int16))
            {
                property.SetValue(obj, Convert.ToInt16(valueAsString), null);
            }
            else if (propertyType == typeof(Int64))
            {
                property.SetValue(obj, Convert.ToInt64(valueAsString), null);
            }
            else if (propertyType == typeof(Double))
            {
                property.SetValue(obj, Convert.ToDouble(valueAsString), null);
            }
            else if (propertyType == typeof(UInt32))
            {
                property.SetValue(obj, Convert.ToUInt32(valueAsString), null);
            }
            else if (propertyType == typeof(UInt16))
            {
                property.SetValue(obj, Convert.ToUInt16(valueAsString), null);
            }
            else if (propertyType == typeof(UInt64))
            {
                property.SetValue(obj, Convert.ToUInt64(valueAsString), null);
            }
            else if (propertyType == typeof(SByte))
            {
                property.SetValue(obj, Convert.ToSByte(valueAsString), null);
            }
            else if (propertyType == typeof(Char))
            {
                property.SetValue(obj, Convert.ToChar(valueAsString), null);
            }
            else if (propertyType == typeof(TimeSpan))
            {
                property.SetValue(obj, TimeSpan.Parse(valueAsString), null);
            }
            else if (propertyType == typeof(Uri))
            {
                property.SetValue(obj, new Uri(valueAsString), null);
            }
            else
            {
                property.SetValue(obj, Convert.ChangeType(value, property.PropertyType));
                //property.SetValue(item, value, null);
            }
        }
    }
}
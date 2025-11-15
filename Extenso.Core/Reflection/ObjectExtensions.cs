using System.Collections;
using System.Reflection;

namespace Extenso.Reflection;

/// <summary>
/// Provides a set of methods for manipulating objects and providing access to object metadata.
/// </summary>
public static class ObjectExtensions
{
    extension<T>(T source)
    {
        /// <summary>
        /// Returns the value of the non-public field with the given name for obj.
        /// </summary>
        /// <param name="fieldName">The name of the non-public field to get the value for.</param>
        /// <returns>The value of the specified non-public field for obj.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public object GetPrivateFieldValue(string fieldName)
        {
            ArgumentNullException.ThrowIfNull(source);

            var type = typeof(T);
            FieldInfo fieldInfo = null;
            while (fieldInfo is null && type is not null)
            {
                var typeInfo = type.GetTypeInfo();
                fieldInfo = typeInfo.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                type = typeInfo.BaseType;
            }

            return fieldInfo is null
                ? throw new ArgumentOutOfRangeException(
                    nameof(fieldName),
                    $"Field {fieldName} was not found in Type {typeof(T).FullName}")
                : fieldInfo.GetValue(source);
        }

        /// <summary>
        /// Returns the value of the non-public property with the given name for obj.
        /// </summary>
        /// <param name="propertyName">The name of the non-public property to get the value for.</param>
        /// <returns>The value of the specified non-public property for obj.</returns>
        public object GetPrivatePropertyValue(string propertyName)
        {
            ArgumentNullException.ThrowIfNull(source);

            var property = typeof(T).GetTypeInfo().GetProperty(
                propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            return property is null
                ? throw new ArgumentOutOfRangeException(
                   nameof(propertyName),
                   $"Property {propertyName} was not found in Type {typeof(T).FullName}")
                : property.GetValue(source, null);
        }

        /// <summary>
        /// Returns the value of the property specified by the given System.Reflection.PropertyInfo for obj.
        /// </summary>
        /// <param name="property">The System.Reflection.PropertyInfo to get the value for.</param>
        /// <returns>The value of the specified property for obj.</returns>
        public object GetPropertyValue(PropertyInfo property) => property.GetValue(source, null);

        /// <summary>
        /// Returns the value of the property with the given name for obj.
        /// </summary>
        /// <param name="propertyName">The name of the property to get the value for.</param>
        /// <returns>The value of the specified property for obj.</returns>
        public object GetPropertyValue(string propertyName)
        {
            var property = source.GetType().GetTypeInfo().GetProperty(propertyName);
            return GetPropertyValue(source, property);
        }

        /// <summary>
        /// Gets a value indicating whether obj has a property with the given name.
        /// </summary>
        /// <param name="propertyName">The name of the property to find in obj.</param>
        /// <returns>true if obj has a property with the given name; otherwise false.</returns>
        public bool HasProperty(string propertyName) =>
            typeof(T).GetTypeInfo().GetProperties().SingleOrDefault(p => p.Name.Equals(propertyName)) is not null;

        /// <summary>
        /// Invokes the extension method with the given name in the given assembly, using the specified parameters.
        /// Use this if your return type is NOT a collection; otherwise use InvokeExtensionMethodForCollection().
        /// </summary>
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
        public object InvokeExtensionMethod(Assembly extensionsAssembly, string methodName, params object[] parameters)
        {
            object[] newParameters = new object[parameters.Length + 1];
            newParameters[0] = source;

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
            object value = methodInfo.Invoke(source, newParameters);

            return value;
        }

        /// <summary>
        /// Invokes the extension method with the given name in the given assembly, using the specified parameters.
        /// Use this if your return type is a collection; otherwise use InvokeExtensionMethod().
        /// </summary>
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
        public IEnumerable<object> InvokeExtensionMethodForCollection(Assembly extensionsAssembly, string methodName, params object[] parameters)
        {
            object[] newParameters = new object[parameters.Length + 1];
            newParameters[0] = source;

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
            object value = methodInfo.Invoke(source, newParameters);

            return value is null ? [] : ((IEnumerable)value).OfType<object>();
        }

        /// <summary>
        /// Invokes the method with the given name, using the specified parameters.
        /// </summary>
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
        public object InvokeMethod(string methodName, params object[] parameters)
        {
            var types = new Type[parameters.Length];

            for (int i = 0; i < types.Length; i++)
            {
                types[i] = parameters[i].GetType();
            }

            var methodInfo = typeof(T).GetTypeInfo().GetMethod(methodName, types);
            object value = methodInfo.Invoke(source, parameters);
            return value;
        }

        /// <summary>
        /// Invokes the non-public method with the given name, using the specified parameters.
        /// </summary>
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
        /// <returns>An object containing the return value of the invoked non-public method.</returns>
        public object InvokePrivateMethod(string methodName, params object[] parameters)
        {
            var types = new Type[parameters.Length];

            for (int i = 0; i < types.Length; i++)
            {
                types[i] = parameters[i].GetType();
            }

            var methodInfo = typeof(T).GetTypeInfo().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance, types);
            object value = methodInfo.Invoke(source, parameters);
            return value;
        }

        /// <summary>
        /// Sets the value of the specified non-public field on the given object.
        /// </summary>
        /// <param name="fieldName">The name of the non-public field to set the value for.</param>
        /// <param name="value">The value to assign to the non-public field.</param>
        public void SetPrivateFieldValue(string fieldName, object value)
        {
            ArgumentNullException.ThrowIfNull(source);

            var type = typeof(T);
            FieldInfo fieldInfo = null;

            while (fieldInfo is null && type is not null)
            {
                var typeInfo = type.GetTypeInfo();
                fieldInfo = typeInfo.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                type = typeInfo.BaseType;
            }

            if (fieldInfo is null)
            {
                throw new ArgumentOutOfRangeException(
                   nameof(fieldName),
                   $"Field {fieldName} was not found in Type {source.GetType().FullName}");
            }

            fieldInfo.SetValue(source, value);
        }

        /// <summary>
        /// Sets the value of the specified non-public property on the given object.
        /// </summary>
        /// <param name="propertyName">The name of the non-public property to set the value for.</param>
        /// <param name="value">The value to assign to the non-public property.</param>
        public void SetPrivatePropertyValue(string propertyName, object value)
        {
            var type = typeof(T);
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.GetProperty(
                propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) is null)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(propertyName),
                    $"Property {propertyName} was not found in Type {typeof(T).FullName}");
            }

            type.InvokeMember(
                propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance,
                null, source,
                [value]);
        }

        /// <summary>
        /// Sets the value of the specified property on the given object.
        /// </summary>
        /// <param name="propertyName">The name of the property to set the value for.</param>
        /// <param name="value">The value to assign to the property.</param>
        public void SetPropertyValue(string propertyName, object value) =>
            SetPropertyValue(source, source.GetType().GetTypeInfo().GetProperty(propertyName), value);

        /// <summary>
        /// Sets the value of the property specified by the given System.Reflection.PropertyInfo for obj.
        /// </summary>
        /// <param name="property">The name of the property to set the value for.</param>
        /// <param name="value">The value to assign to the property.</param>
        public void SetPropertyValue(PropertyInfo property, object value)
        {
            var propertyType = property.PropertyType;
            string valueAsString = value.ToString();

            if (propertyType == typeof(string))
            {
                property.SetValue(source, valueAsString, null);
            }
            else if (propertyType == typeof(int))
            {
                property.SetValue(source, Convert.ToInt32(valueAsString), null);
            }
            else if (propertyType == typeof(Guid))
            {
                property.SetValue(source, new Guid(valueAsString), null);
            }
            else if (propertyType.GetTypeInfo().IsEnum)
            {
                object enumValue = Enum.Parse(propertyType, valueAsString);
                property.SetValue(source, enumValue, null);
            }
            else if (propertyType == typeof(bool))
            {
                property.SetValue(source, Convert.ToBoolean(valueAsString), null);
            }
            else if (propertyType == typeof(DateTime))
            {
                property.SetValue(source, Convert.ToDateTime(valueAsString), null);
            }
            else if (propertyType == typeof(float))
            {
                property.SetValue(source, Convert.ToSingle(valueAsString), null);
            }
            else if (propertyType == typeof(decimal))
            {
                property.SetValue(source, Convert.ToDecimal(valueAsString), null);
            }
            else if (propertyType == typeof(byte))
            {
                property.SetValue(source, Convert.ToByte(valueAsString), null);
            }
            else if (propertyType == typeof(short))
            {
                property.SetValue(source, Convert.ToInt16(valueAsString), null);
            }
            else if (propertyType == typeof(long))
            {
                property.SetValue(source, Convert.ToInt64(valueAsString), null);
            }
            else if (propertyType == typeof(double))
            {
                property.SetValue(source, Convert.ToDouble(valueAsString), null);
            }
            else if (propertyType == typeof(uint))
            {
                property.SetValue(source, Convert.ToUInt32(valueAsString), null);
            }
            else if (propertyType == typeof(ushort))
            {
                property.SetValue(source, Convert.ToUInt16(valueAsString), null);
            }
            else if (propertyType == typeof(ulong))
            {
                property.SetValue(source, Convert.ToUInt64(valueAsString), null);
            }
            else if (propertyType == typeof(sbyte))
            {
                property.SetValue(source, Convert.ToSByte(valueAsString), null);
            }
            else if (propertyType == typeof(char))
            {
                property.SetValue(source, Convert.ToChar(valueAsString), null);
            }
            else if (propertyType == typeof(TimeSpan))
            {
                property.SetValue(source, TimeSpan.Parse(valueAsString), null);
            }
            else if (propertyType == typeof(Uri))
            {
                property.SetValue(source, new Uri(valueAsString), null);
            }
            else
            {
                property.SetValue(source, Convert.ChangeType(value, property.PropertyType));
                //property.SetValue(item, value, null);
            }
        }
    }
}
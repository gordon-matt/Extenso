using System.Collections;
using System.Reflection;

namespace Extenso.Reflection;

/// <summary>
/// Provides a set of static methods for manipulating objects and providing access to object metadata.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Returns the value of the non-public field with the given name for obj.
    /// </summary>
    /// <typeparam name="T">The type of obj.</typeparam>
    /// <param name="obj">The object to get the non-public field value from.</param>
    /// <param name="fieldName">The name of the non-public field to get the value for.</param>
    /// <returns>The value of the specified non-public field for obj.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static object GetPrivateFieldValue<T>(this T obj, string fieldName)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

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
                "fieldName",
                $"Field {fieldName} was not found in Type {typeof(T).FullName}")
            : fieldInfo.GetValue(obj);
    }

    /// <summary>
    /// Returns the value of the non-public property with the given name for obj.
    /// </summary>
    /// <typeparam name="T">The type of obj.</typeparam>
    /// <param name="obj">The object to get the non-public property value from.</param>
    /// <param name="propertyName">The name of the non-public property to get the value for.</param>
    /// <returns>The value of the specified non-public property for obj.</returns>
    public static object GetPrivatePropertyValue<T>(this T obj, string propertyName)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        var property = typeof(T).GetTypeInfo().GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        return property is null
            ? throw new ArgumentOutOfRangeException(
               "propertyName",
               $"Property {propertyName} was not found in Type {typeof(T).FullName}")
            : property.GetValue(obj, null);
    }

    /// <summary>
    /// Returns the value of the property specified by the given System.Reflection.PropertyInfo for obj.
    /// </summary>
    /// <typeparam name="T">The type of obj.</typeparam>
    /// <param name="obj">The object to get the property value from.</param>
    /// <param name="property">The System.Reflection.PropertyInfo to get the value for.</param>
    /// <returns>The value of the specified property for obj.</returns>
    public static object GetPropertyValue<T>(this T obj, PropertyInfo property) => property.GetValue(obj, null);

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
    public static bool HasProperty<T>(this T obj, string propertyName) =>
        typeof(T).GetTypeInfo().GetProperties().SingleOrDefault(p => p.Name.Equals(propertyName)) is not null;

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
        object[] newParameters = new object[parameters.Length + 1];
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
        object value = methodInfo.Invoke(obj, newParameters);

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
        object[] newParameters = new object[parameters.Length + 1];
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
        object value = methodInfo.Invoke(obj, newParameters);

        return value is null ? Enumerable.Empty<object>() : ((IEnumerable)value).OfType<object>();
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
        object value = methodInfo.Invoke(obj, parameters);
        return value;
    }

    /// <summary>
    /// Invokes the non-public method with the given name, using the specified parameters.
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
    /// <returns>An object containing the return value of the invoked non-public method.</returns>
    public static object InvokePrivateMethod<T>(this T obj, string methodName, params object[] parameters)
    {
        var types = new Type[parameters.Length];

        for (int i = 0; i < types.Length; i++)
        {
            types[i] = parameters[i].GetType();
        }

        var methodInfo = typeof(T).GetTypeInfo().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance, types);
        object value = methodInfo.Invoke(obj, parameters);
        return value;
    }

    /// <summary>
    /// Sets the value of the specified non-public field on the given object.
    /// </summary>
    /// <typeparam name="T">The type of obj.</typeparam>
    /// <param name="obj">The object whose non-public field value will be set.</param>
    /// <param name="fieldName">The name of the non-public field to set the value for.</param>
    /// <param name="value">The value to assign to the non-public field.</param>
    public static void SetPrivateFieldValue<T>(this T obj, string fieldName, object value)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

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
               $"Field {fieldName} was not found in Type {obj.GetType().FullName}");
        }

        fieldInfo.SetValue(obj, value);
    }

    /// <summary>
    /// Sets the value of the specified non-public property on the given object.
    /// </summary>
    /// <typeparam name="T">The type of obj.</typeparam>
    /// <param name="obj">The object whose non-public property value will be set.</param>
    /// <param name="propertyName">The name of the non-public property to set the value for.</param>
    /// <param name="value">The value to assign to the non-public property.</param>
    public static void SetPrivatePropertyValue<T>(this T obj, string propertyName, object value)
    {
        var type = typeof(T);
        var typeInfo = type.GetTypeInfo();

        if (typeInfo.GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) is null)
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
    public static void SetPropertyValue<T>(this T obj, string propertyName, object value) =>
        SetPropertyValue(obj, obj.GetType().GetTypeInfo().GetProperty(propertyName), value);

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

        if (propertyType == typeof(string))
        {
            property.SetValue(obj, valueAsString, null);
        }
        else if (propertyType == typeof(int))
        {
            property.SetValue(obj, Convert.ToInt32(valueAsString), null);
        }
        else if (propertyType == typeof(Guid))
        {
            property.SetValue(obj, new Guid(valueAsString), null);
        }
        else if (propertyType.GetTypeInfo().IsEnum)
        {
            object enumValue = Enum.Parse(propertyType, valueAsString);
            property.SetValue(obj, enumValue, null);
        }
        else if (propertyType == typeof(bool))
        {
            property.SetValue(obj, Convert.ToBoolean(valueAsString), null);
        }
        else if (propertyType == typeof(DateTime))
        {
            property.SetValue(obj, Convert.ToDateTime(valueAsString), null);
        }
        else if (propertyType == typeof(float))
        {
            property.SetValue(obj, Convert.ToSingle(valueAsString), null);
        }
        else if (propertyType == typeof(decimal))
        {
            property.SetValue(obj, Convert.ToDecimal(valueAsString), null);
        }
        else if (propertyType == typeof(byte))
        {
            property.SetValue(obj, Convert.ToByte(valueAsString), null);
        }
        else if (propertyType == typeof(short))
        {
            property.SetValue(obj, Convert.ToInt16(valueAsString), null);
        }
        else if (propertyType == typeof(long))
        {
            property.SetValue(obj, Convert.ToInt64(valueAsString), null);
        }
        else if (propertyType == typeof(double))
        {
            property.SetValue(obj, Convert.ToDouble(valueAsString), null);
        }
        else if (propertyType == typeof(uint))
        {
            property.SetValue(obj, Convert.ToUInt32(valueAsString), null);
        }
        else if (propertyType == typeof(ushort))
        {
            property.SetValue(obj, Convert.ToUInt16(valueAsString), null);
        }
        else if (propertyType == typeof(ulong))
        {
            property.SetValue(obj, Convert.ToUInt64(valueAsString), null);
        }
        else if (propertyType == typeof(sbyte))
        {
            property.SetValue(obj, Convert.ToSByte(valueAsString), null);
        }
        else if (propertyType == typeof(char))
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
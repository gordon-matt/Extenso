using System.Reflection;

namespace Extenso.Reflection;

/// <summary>
/// Provides a set of static methods for parsing strings
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// If the specified type has a Parse(string) method, it will be invoked on the given input and the result returned.
    /// </summary>
    /// <param name="input">The System.String to parse.</param>
    /// <param name="type">The type to examine for a Parse(string) method.</param>
    /// <returns>The result, if the given type has a Parse(string) method; otherwise null.</returns>
    public static object ParseOrDefault(this string input, Type type)
    {
        var parseMethod = type.GetTypeInfo().GetMethod("Parse", new Type[] { typeof(string) });

        return parseMethod?.Invoke(null, [input]);
    }

    /// <summary>
    /// If the specified type has a Parse(string) method, it will be invoked on the given input and the result returned.
    /// </summary>
    /// <typeparam name="T">The type to examine for a Parse(string) method.</typeparam>
    /// <param name="input">The System.String to parse.</param>
    /// <returns>The result, if the given type has a Parse(string) method; otherwise default(T).</returns>
    public static T ParseOrDefault<T>(this string input) => input.ParseOrDefault(default(T));

    /// <summary>
    /// If the specified type has a Parse(string) method, it will be invoked on the given input and the result returned.
    /// </summary>
    /// <typeparam name="T">The type to examine for a Parse(string) method.</typeparam>
    /// <param name="input">The System.String to parse.</param>
    /// <param name="defaultValue">A value to return in case the specified type has no Parse(string) method.</param>
    /// <returns>The result, if the given type has a Parse(string) method; otherwise defaultValue.</returns>
    public static T ParseOrDefault<T>(this string input, T defaultValue)
    {
        var type = typeof(T);
        var parseMethod = type.GetTypeInfo().GetMethod("Parse", new Type[] { typeof(string) });

        if (parseMethod != null)
        {
            object value = parseMethod.Invoke(null, [input]);
            return value is T t ? t : defaultValue;
        }
        else { return defaultValue; }
    }

    /// <summary>
    /// If the specified type has a TryParse(string, out Type) method, it will be invoked on the given input and the result returned.
    /// </summary>
    /// <typeparam name="T">The type to examine for a TryParse(string, out Type) method.</typeparam>
    /// <param name="input">The System.String to parse.</param>
    /// <param name="result">
    /// When this method returns, contains the value equivalent of that represented in input,
    /// if the conversion succeeded, or default(T) if the conversion failed. The conversion
    /// fails if the input parameter is null or System.String.Empty, is not of the correct
    /// format, or represents an invalid value for the specified type. This parameter is passed
    /// uninitialized; any value originally supplied in result will be overwritten.
    /// </param>
    /// <returns>true if input was converted successfully; otherwise, false.</returns>
    public static bool TryParseOrDefault<T>(this string input, out T result) => input.TryParseOrDefault(out result, default);

    /// <summary>
    /// If the specified type has a TryParse(string, out Type) method, it will be invoked on the given input and the result returned.
    /// </summary>
    /// <typeparam name="T">The type to examine for a TryParse(string, out Type) method.</typeparam>
    /// <param name="input">The System.String to parse.</param>
    /// <param name="result">
    /// When this method returns, contains the value equivalent of that represented in input,
    /// if the conversion succeeded, or defaultValue if the conversion failed. The conversion
    /// fails if the input parameter is null or System.String.Empty, is not of the correct
    /// format, or represents an invalid value for the specified type. This parameter is passed
    /// uninitialized; any value originally supplied in result will be overwritten.
    /// </param>
    /// <param name="defaultValue">A value to return in case the specified type has no TryParse(string, out Type) method.</param>
    /// <returns>true if input was converted successfully; otherwise, false.</returns>
    public static bool TryParseOrDefault<T>(this string input, out T result, T defaultValue)
    {
        result = defaultValue;

        var type = typeof(T);
        var parseMethod = type.GetTypeInfo().GetMethod(
            "TryParse",
            new Type[] { typeof(string), typeof(T).MakeByRefType() });

        if (parseMethod != null)
        {
            object[] parameters = [input, result];
            object value = parseMethod.Invoke(null, parameters);

            if (value is bool successful)
            {
                if (successful)
                {
                    result = (T)parameters[1];
                    return true;
                }
            }
        }
        return false;
    }
}
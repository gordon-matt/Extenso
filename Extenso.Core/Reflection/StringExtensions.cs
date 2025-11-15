using System.Reflection;

namespace Extenso.Reflection;

/// <summary>
/// Provides a set of static methods for parsing strings
/// </summary>
public static class StringExtensions
{
    extension(string source)
    {
        /// <summary>
        /// If the specified type has a Parse(string) method, it will be invoked on the given input and the result returned.
        /// </summary>
        /// <param name="type">The type to examine for a Parse(string) method.</param>
        /// <returns>The result, if the given type has a Parse(string) method; otherwise null.</returns>
        public object ParseOrDefault(Type type)
        {
            var parseMethod = type.GetTypeInfo().GetMethod("Parse", [typeof(string)]);

            return parseMethod?.Invoke(null, [source]);
        }

        /// <summary>
        /// If the specified type has a Parse(string) method, it will be invoked on the given input and the result returned.
        /// </summary>
        /// <typeparam name="T">The type to examine for a Parse(string) method.</typeparam>
        /// <returns>The result, if the given type has a Parse(string) method; otherwise default(T).</returns>
        public T ParseOrDefault<T>() => source.ParseOrDefault(default(T));

        /// <summary>
        /// If the specified type has a Parse(string) method, it will be invoked on the given input and the result returned.
        /// </summary>
        /// <typeparam name="T">The type to examine for a Parse(string) method.</typeparam>
        /// <param name="defaultValue">A value to return in case the specified type has no Parse(string) method.</param>
        /// <returns>The result, if the given type has a Parse(string) method; otherwise defaultValue.</returns>
        public T ParseOrDefault<T>(T defaultValue)
        {
            var type = typeof(T);
            var parseMethod = type.GetTypeInfo().GetMethod("Parse", [typeof(string)]);

            if (parseMethod is not null)
            {
                object value = parseMethod.Invoke(null, [source]);
                return value is T t ? t : defaultValue;
            }
            else { return defaultValue; }
        }

        /// <summary>
        /// If the specified type has a TryParse(string, out Type) method, it will be invoked on the given input and the result returned.
        /// </summary>
        /// <typeparam name="T">The type to examine for a TryParse(string, out Type) method.</typeparam>
        /// <param name="result">
        /// When this method returns, contains the value equivalent of that represented in input,
        /// if the conversion succeeded, or default(T) if the conversion failed. The conversion
        /// fails if the input parameter is null or System.String.Empty, is not of the correct
        /// format, or represents an invalid value for the specified type. This parameter is passed
        /// uninitialized; any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns>true if input was converted successfully; otherwise, false.</returns>
        public bool TryParseOrDefault<T>(out T result) => source.TryParseOrDefault(out result, default);

        /// <summary>
        /// If the specified type has a TryParse(string, out Type) method, it will be invoked on the given input and the result returned.
        /// </summary>
        /// <typeparam name="T">The type to examine for a TryParse(string, out Type) method.</typeparam>
        /// <param name="result">
        /// When this method returns, contains the value equivalent of that represented in input,
        /// if the conversion succeeded, or defaultValue if the conversion failed. The conversion
        /// fails if the input parameter is null or System.String.Empty, is not of the correct
        /// format, or represents an invalid value for the specified type. This parameter is passed
        /// uninitialized; any value originally supplied in result will be overwritten.
        /// </param>
        /// <param name="defaultValue">A value to return in case the specified type has no TryParse(string, out Type) method.</param>
        /// <returns>true if input was converted successfully; otherwise, false.</returns>
        public bool TryParseOrDefault<T>(out T result, T defaultValue)
        {
            result = defaultValue;

            var type = typeof(T);
            var parseMethod = type.GetTypeInfo().GetMethod(
                "TryParse",
                [typeof(string), typeof(T).MakeByRefType()]);

            if (parseMethod is not null)
            {
                object[] parameters = [source, result];
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
}
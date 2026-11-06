using System.Collections.Specialized;

namespace Extenso.KendoGridBinder.Extensions;

/// <summary>
/// Provides extension methods for name value collections.
/// </summary>
public static class NameValueCollectionExtensions
{
    extension(NameValueCollection source)
    {
        /// <summary>
        /// Converts the value to the dictionary.
        /// </summary>
        /// <returns>The dictionary.</returns>
        public Dictionary<string, string> ToDictionary() => source
            ?.Cast<string>()
            .Select(s => new { Key = s, Value = source.Get(s) })
            .ToDictionary(p => p.Key, p => p.Value);

        /// <summary>
        /// Gets the query value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The query value.</returns>
        public T GetQueryValue<T>(string key, T defaultValue = default)
        {
            string stringValue = source[key];

            return !string.IsNullOrEmpty(stringValue)
                ? (T)TypeExtensions.ChangeType(stringValue, typeof(T))
                : defaultValue;
        }
    }
}
using System.Collections.Specialized;

namespace Extenso.KendoGridBinder.Extensions;

public static class NameValueCollectionExtensions
{
    extension(NameValueCollection source)
    {
        public Dictionary<string, string> ToDictionary() => source
            ?.Cast<string>()
            .Select(s => new { Key = s, Value = source.Get(s) })
            .ToDictionary(p => p.Key, p => p.Value);

        public T GetQueryValue<T>(string key, T defaultValue = default)
        {
            string stringValue = source[key];

            return !string.IsNullOrEmpty(stringValue)
                ? (T)TypeExtensions.ChangeType(stringValue, typeof(T))
                : defaultValue;
        }
    }
}
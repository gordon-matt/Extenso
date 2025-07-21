using System.Collections.Specialized;

namespace Extenso.KendoGridBinder.Extensions;

public static class NameValueCollectionExtensions
{
    public static Dictionary<string, string> ToDictionary(this NameValueCollection source) => source?.Cast<string>().Select(s => new { Key = s, Value = source.Get(s) }).ToDictionary(p => p.Key, p => p.Value);

    public static T GetQueryValue<T>(this NameValueCollection queryString, string key, T defaultValue = default)
    {
        string stringValue = queryString[key];

        return !string.IsNullOrEmpty(stringValue) ? (T)TypeExtensions.ChangeType(stringValue, typeof(T)) : defaultValue;
    }
}
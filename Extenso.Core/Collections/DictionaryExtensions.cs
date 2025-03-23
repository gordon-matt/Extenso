using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Extenso.Collections;

/// <summary>
/// Provides a set of static methods for querying and manipulating objects that implement System.Collections.Generic.IDictionary`2.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Adds the given key and value to the dictionary or updates the value if the key already exists.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="source">The System.Collections.IDictionary`2 that will have the item added or updated.</param>
    /// <param name="key">The object to use as the key of the element.</param>
    /// <param name="value">The object to use as the value of the element.</param>
    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
    {
        if (source.ContainsKey(key))
        {
            source[key] = value;
        }
        else
        {
            source.Add(key, value);
        }
    }

    /// <summary>
    /// If there is an entry with the given key, the value is returned. Otherwise, a new entry is created and the given value is still returned.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="source">The System.Collections.IDictionary`2 to retrieve the item from and insert to if necessary.</param>
    /// <param name="key">The object to use as the key of the element.</param>
    /// <param name="value">The object to use as the value of the element.</param>
    /// <returns>The value associated with the given key.</returns>
    public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
    {
        if (!source.TryGetValue(key, out var result))
        {
            result = value;
            source.Add(key, result);
        }
        return result;
    }

    /// <summary>
    /// Inverts the elements in the entire System.Collections.Generic.IDictionary`2 so that the values become the keys and the keys become the values.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="source">The System.Collections.IDictionary`2 to inverse.</param>
    /// <returns>A new System.Collections.Generic.IDictionary`2 that is the inverse of the original.</returns>
    public static Dictionary<TValue, TKey> Invert<TKey, TValue>(this IDictionary<TKey, TValue> source) => source.ToDictionary(k => k.Value, k => k.Key);

    /// <summary>
    /// Produces a new System.Collections.Specialized.NameValueCollection containing the keys and values from the given System.Collections.IDictionary`2.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="source">The System.Collections.IDictionary`2 to create a System.Collections.Specialized.NameValueCollection from.</param>
    /// <returns>A System.Collections.Specialized.NameValueCollection containing the keys and values from source.</returns>
    public static NameValueCollection ToNameValueCollection<TKey, TValue>(this IDictionary<TKey, TValue> source)
    {
        var nameValueCollection = new NameValueCollection();

        foreach (var value in source)
        {
            nameValueCollection.Add(value.Key.ToString(), value.Value.ToString());
        }

        return nameValueCollection;
    }

    /// <summary>
    /// Produces the set union of two dictionaries.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="source">The System.Collections.IDictionary`2 whose distinct elements form the first set for the union.</param>
    /// <param name="other">The System.Collections.IDictionary`2 whose distinct elements form the second set for the union.</param>
    /// <returns>A new System.Collections.Dictionary`2 that contains the elements from both input sequences, excluding duplicates.</returns>
    public static Dictionary<TKey, TValue> Union<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> other)
    {
        var result = new Dictionary<TKey, TValue>(source);
        foreach (var kv in other)
        {
            var value = result.GetOrCreate(kv.Key, kv.Value);
        }
        return result;
    }
}
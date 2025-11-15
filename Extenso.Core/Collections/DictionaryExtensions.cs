using System.Collections.Specialized;

namespace Extenso.Collections;

/// <summary>
/// Provides a set of static methods for querying and manipulating objects that implement System.Collections.Generic.IDictionary`2.
/// </summary>
public static class DictionaryExtensions
{
    extension<TKey, TValue>(IDictionary<TKey, TValue> source)
    {
        /// <summary>
        /// Adds the given key and value to the dictionary or updates the value if the key already exists.
        /// </summary>
        /// <param name="key">The object to use as the key of the element.</param>
        /// <param name="value">The object to use as the value of the element.</param>
        public void AddOrUpdate(TKey key, TValue value)
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
        /// <param name="key">The object to use as the key of the element.</param>
        /// <param name="value">The object to use as the value of the element.</param>
        /// <returns>The value associated with the given key.</returns>
        public TValue GetOrCreate(TKey key, TValue value)
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
        /// <returns>A new System.Collections.Generic.IDictionary`2 that is the inverse of the original.</returns>
        public Dictionary<TValue, TKey> Invert() => source.ToDictionary(k => k.Value, k => k.Key);

        /// <summary>
        /// Produces a new System.Collections.Specialized.NameValueCollection containing the keys and values from the given System.Collections.IDictionary`2.
        /// </summary>
        /// <returns>A System.Collections.Specialized.NameValueCollection containing the keys and values from source.</returns>
        public NameValueCollection ToNameValueCollection()
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
        /// <param name="other">The System.Collections.IDictionary`2 whose distinct elements form the second set for the union.</param>
        /// <returns>A new System.Collections.Dictionary`2 that contains the elements from both input sequences, excluding duplicates.</returns>
        public Dictionary<TKey, TValue> Union(IDictionary<TKey, TValue> other)
        {
            var result = new Dictionary<TKey, TValue>(source);
            foreach (var kv in other)
            {
                _ = result.GetOrCreate(kv.Key, kv.Value);
            }
            return result;
        }
    }
}
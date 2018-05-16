using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Extenso.Collections
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            TValue result;
            if (!dictionary.TryGetValue(key, out result))
            {
                result = value;
                dictionary.Add(key, result);
            }
            return result;
        }

        public static Dictionary<TValue, TKey> Inverse<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return dictionary.ToDictionary(k => k.Value, k => k.Key);
        }

        public static NameValueCollection ToNameValueCollection(this Dictionary<string, string> dictionary)
        {
            var nameValueCollection = new NameValueCollection();

            foreach (var value in dictionary)
            {
                nameValueCollection.Add(value.Key, value.Value);
            }

            return nameValueCollection;
        }

        public static NameValueCollection ToNameValueCollection<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var nameValueCollection = new NameValueCollection();

            foreach (var value in dictionary)
            {
                nameValueCollection.Add(value.Key.ToString(), value.Value.ToString());
            }

            return nameValueCollection;
        }

        public static Dictionary<TKey, TValue> Union<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> other)
        {
            var result = new Dictionary<TKey, TValue>(dictionary);
            foreach (KeyValuePair<TKey, TValue> kv in other)
            {
                TValue value = result.GetOrCreate(kv.Key, kv.Value);
            }
            return result;
        }
    }
}
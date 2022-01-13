using System;
using System.Collections.Generic;
using System.Linq;

namespace Extenso.Collections
{
    /// <summary>
    /// Provides a set of static methods for querying and manipulating objects that implement System.Collections.Generic.ICollection`1.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds an item to the collection if predicate resolves to true.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.ICollection`1 to add to if the predicate resolved to true.</param>
        /// <param name="obj">The element to add if predicate resolves to true.</param>
        /// <param name="predicate">The predicate.</param>
        public static void AddIf<T>(this ICollection<T> source, T obj, Func<T, bool> predicate)
        {
            if (predicate(obj))
            {
                source.Add(obj);
            }
        }

        /// <summary>
        ///  Adds the elements of the specified collection to the end of the System.Collections.Generic.ICollection`1.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.ICollection`1 to add elements to.</param>
        /// <param name="values">The collection whose elements should be added to the end of source.</param>
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                source.Add(value);
            }
        }

        public static bool RemoveFirst<T>(this ICollection<T> source)
        {
            if (source.IsNullOrEmpty())
            {
                return false;
            }

            var item = source.First();
            return source.Remove(item);
        }

        public static bool RemoveLast<T>(this ICollection<T> source)
        {
            if (source.IsNullOrEmpty())
            {
                return false;
            }

            var item = source.Last();
            return source.Remove(item);
        }

        /// <summary>
        /// Removes a range of elements from the System.Collections.Generic.ICollection`1.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.ICollection`1 to remove elements from.</param>
        /// <param name="values">The collection whose elements should be removed from source if found therein.</param>
        public static void RemoveRange<T>(this ICollection<T> source, IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                if (source.Contains(value))
                {
                    source.Remove(value);
                }
            }
        }
    }
}
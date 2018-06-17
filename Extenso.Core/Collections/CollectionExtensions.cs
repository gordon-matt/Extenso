using System;
using System.Collections.Generic;

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
        /// <param name="item">The element to add if predicate resolves to true.</param>
        /// <param name="predicate">The predicate.</param>
        public static void AddIf<T>(this ICollection<T> source, T item, Func<T, bool> predicate)
        {
            if (predicate(item))
            {
                source.Add(item);
            }
        }

        /// <summary>
        ///  Adds the elements of the specified collection to the end of the System.Collections.Generic.ICollection`1.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.ICollection`1 to add elements to.</param>
        /// <param name="collection">The collection whose elements should be added to the end of source.</param>
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                source.Add(item);
            }
        }

        /// <summary>
        /// Removes a range of elements from the System.Collections.Generic.ICollection`1.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.ICollection`1 to remove elements from.</param>
        /// <param name="collection">The collection whose elements should be removed from source if found therein.</param>
        public static void RemoveRange<T>(this ICollection<T> source, IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                if (source.Contains(item))
                {
                    source.Remove(item);
                }
            }
        }
    }
}
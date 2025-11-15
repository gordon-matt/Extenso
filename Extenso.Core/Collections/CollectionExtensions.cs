namespace Extenso.Collections;

/// <summary>
/// Provides a set of static methods for querying and manipulating objects that implement System.Collections.Generic.ICollection`1.
/// </summary>
public static class CollectionExtensions
{
    extension<T>(ICollection<T> source)
    {
        /// <summary>
        /// Adds an item to the collection if predicate resolves to true.
        /// </summary>
        /// <param name="obj">The element to add if predicate resolves to true.</param>
        /// <param name="predicate">The predicate.</param>
        public void AddIf(T obj, Func<T, bool> predicate)
        {
            if (predicate(obj))
            {
                source.Add(obj);
            }
        }

        /// <summary>
        ///  Adds the elements of the specified collection to the end of the System.Collections.Generic.ICollection`1.
        /// </summary>
        /// <param name="values">The collection whose elements should be added to the end of source.</param>
        public void AddRange(IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                source.Add(value);
            }
        }

        /// <summary>
        /// Removes the first element from the System.Collections.Generic.ICollection`1.
        /// </summary>
        /// <returns></returns>
        public bool RemoveFirst()
        {
            if (source.IsNullOrEmpty())
            {
                return false;
            }

            var item = source.First();
            return source.Remove(item);
        }

        /// <summary>
        /// Removes the last element from the System.Collections.Generic.ICollection`1.
        /// </summary>
        /// <returns></returns>
        public bool RemoveLast()
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
        /// <param name="values">The collection whose elements should be removed from source if found therein.</param>
        public void RemoveRange(IEnumerable<T> values)
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
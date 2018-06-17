using System.Collections.Generic;

namespace Extenso.Collections.Generic
{
    /// <summary>
    /// Provides a set of extension methods for objects inheriting from or otherwise related to Extenso.Collections.Generic.IPagedCollection`1
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Creates an Extenso.Collections.Generic.IPagedCollection`1 from a System.Collections.Generic.IEnumerable`1.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create an Extenso.Collections.Generic.IPagedCollection`1 from</param>
        /// <param name="pageIndex">The page index</param>
        /// <param name="pageSize">The page size</param>
        /// <param name="itemCount">The total number of items in source</param>
        /// <returns></returns>
        public static IPagedCollection<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int itemCount)
        {
            return new PagedList<T>(source, pageIndex - 1, pageSize, itemCount);
        }
    }
}
namespace Extenso.Collections.Generic;

/// <summary>
/// Provides a set of extension methods for objects inheriting from or otherwise related to Extenso.Collections.Generic.IPagedCollection`1
/// </summary>
public static class EnumerableExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        /// <summary>
        /// Creates an Extenso.Collections.Generic.IPagedCollection`1 from a System.Collections.Generic.IEnumerable`1.
        /// </summary>
        /// <param name="pageIndex">The page index</param>
        /// <param name="pageSize">The page size</param>
        /// <param name="itemCount">The total number of items in source</param>
        /// <returns></returns>
        public IPagedCollection<T> ToPagedList(int pageIndex, int pageSize, int itemCount) =>
            new PagedList<T>(source, pageIndex - 1, pageSize, itemCount);
    }
}
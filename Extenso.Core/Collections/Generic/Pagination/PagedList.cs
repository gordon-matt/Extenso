using System;
using System.Collections.Generic;
using System.Linq;

namespace Extenso.Collections.Generic
{
    /// <summary>
    /// Represents a System.Collections.Generic.List`1 which implements Extenso.Collections.Generic.IPagedCollection`1
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>, IPagedCollection<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of Extenso.Collections.Generic.PagedList`1 that copies all items from source and calculates
        /// PageIndex, PageSize, ItemCount and PageCount from that.
        /// </summary>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a Extenso.Collections.Generic.PagedList`1 from.</param>
        public PagedList(IEnumerable<T> source)
        {
            AddRange(source);
            PageIndex = 1;
            PageSize = Count;
            ItemCount = Count;
            PageCount = 1;
        }

        /// <summary>
        /// Initializes a new instance of Extenso.Collections.Generic.PagedList`1 that copies all items from source, but where ItemCount
        /// is determined by the itemCount parameter. The source parameter need only contain those items for the given pageIndex.
        /// </summary>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a Extenso.Collections.Generic.PagedList`1 from.</param>
        /// <param name="pageIndex">The page index</param>
        /// <param name="pageSize">The page size</param>
        /// <param name="itemCount">The total number of items in source</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int itemCount)
        {
            ItemCount = itemCount;
            PageCount = (int)Math.Ceiling((double)itemCount / pageSize);
            PageIndex = pageIndex;
            PageSize = pageSize;

            AddRange(source);
        }

        /// <summary>
        /// Initializes a new instance of Extenso.Collections.Generic.PagedList`1 that copies only the number of items specified by pageSize
        /// from the given System.Linq.IQueryable`1 from a point in the collection determined by pageIndex.
        /// </summary>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a Extenso.Collections.Generic.PagedList`1 from.</param>
        /// <param name="pageIndex">The page index</param>
        /// <param name="pageSize">The page size</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int itemCount = source.Count();
            ItemCount = itemCount;
            PageCount = (int)Math.Ceiling((double)itemCount / pageSize);

            PageIndex = pageIndex;
            PageSize = pageSize;

            AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        #endregion Constructors

        #region IPagedList<T> Members

        /// <summary>
        /// The page index
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        ///  The page size
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        ///The total number of items in source
        /// </summary>
        public int ItemCount { get; private set; }

        /// <summary>
        /// The total number of pages
        /// </summary>
        public int PageCount { get; private set; }

        /// <summary>
        /// True if PageIndex is greater than 0. Otherwise, false.
        /// </summary>
        public bool HasPreviousPage => (PageIndex > 1);

        /// <summary>
        /// True if PageIndex is less than PageCount. Otherwise, false.
        /// </summary>
        public bool HasNextPage => (PageIndex < PageCount);

        #endregion IPagedList<T> Members
    }
}
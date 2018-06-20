using System.Collections;
using System.Collections.Generic;

namespace Extenso.Collections.Generic
{
    /// <summary>
    /// Defines properties for paging through a collection
    /// </summary>
    public interface IPagedCollection : IEnumerable
    {
        /// <summary>
        /// The page index
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        ///  The page size
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// The total number of items in source
        /// </summary>
        int ItemCount { get; }

        /// <summary>
        /// The total number of pages
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// True if PageIndex is greater than 0. Otherwise, false.
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// True if PageIndex is less than PageCount. Otherwise, false.
        /// </summary>
        bool HasNextPage { get; }
    }

    /// <summary>
    /// A generic IPagedCollection
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public interface IPagedCollection<T> : IPagedCollection, ICollection<T>
    {
    }
}
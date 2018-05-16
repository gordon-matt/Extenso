using System;
using System.Collections.Generic;
using System.Linq;

namespace Extenso.Collections.Generic
{
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int itemCount = source.Count();
            ItemCount = itemCount;
            PageCount = (int)Math.Ceiling((double)itemCount / pageSize);

            PageIndex = pageIndex;
            PageSize = pageSize;

            AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        public PagedList(IEnumerable<T> source)
        {
            AddRange(source);
            PageIndex = 1;
            PageSize = Count;
            ItemCount = Count;
            PageCount = 1;
        }

        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int itemCount)
        {
            ItemCount = itemCount;
            PageCount = (int)Math.Ceiling((double)itemCount / pageSize);
            PageIndex = pageIndex;
            PageSize = pageSize;

            AddRange(source);
        }

        #region IPagedList<T> Members

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public int ItemCount { get; private set; }

        public int PageCount { get; private set; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 1); }
        }

        public bool HasNextPage
        {
            get { return (PageIndex < PageCount); }
        }

        #endregion IPagedList<T> Members
    }

    public class PagedList<TParent, T> : PagedList<T>
    {
        public PagedList(TParent parent, IEnumerable<T> source)
            : base(source)
        {
            Parent = parent;
        }

        public PagedList(TParent parent, IEnumerable<T> source, int pageIndex, int pageSize, int itemCount)
            : base(source, pageIndex, pageSize, itemCount)
        {
            Parent = parent;
        }

        public TParent Parent { get; set; }
    }
}
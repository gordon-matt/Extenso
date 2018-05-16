using System.Collections.Generic;

namespace Extenso.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> enumerable, int pageIndex, int pageSize, int itemCount)
        {
            return new PagedList<T>(enumerable, pageIndex - 1, pageSize, itemCount);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;

//using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Extenso.Collections
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Determines whether this collection contains any of the specified values
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">This collection</param>
        /// <param name="items">The values to compare</param>
        /// <returns>true if the collection contains any of the specified values, otherwise false</returns>
        public static bool ContainsAny<T>(this IEnumerable<T> source, params T[] items)
        {
            return items.Any(source.Contains);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool ContainsAny<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            return items.Any(source.Contains);
        }

        /// <summary>
        /// Performs the specified action on each element of the System.Collections.Generic.IEnumerable&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">This instance of System.Collections.Generic.IEnumerable&lt;T&gt;.</param>
        /// <param name="action">The System.Action&lt;T&gt; delegate to perform on each element of the System.Collections.Generic.IEnumerable&lt;T&gt;.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Indicates whether the specified System.Collections.Generic.IEnumerable&lt;T&gt; is null or empty
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">This instance of System.Collections.Generic.IEnumerable&lt;T&gt;.</param>
        /// <returns>true if the System.Collections.Generic.IEnumerable&lt;T&gt; is null or empty; otherwise, false.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.FastAny();
            //return enumerable == null || !enumerable.Any();
        }

        /// <summary>
        /// <para>Returns all elements of this IEnumerable&lt;T&gt; in a single System.String.</para>
        /// <para>Elements are separated by a comma.</para>
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">This instance of System.Collections.Generic.IEnumerable&lt;T&gt;.</param>
        /// <returns>System.String containing elements from specified IEnumerable&lt;T&gt;.</returns>
        public static string Join<T>(this IEnumerable<T> source)
        {
            return source.Join(",");
        }

        /// <summary>
        /// <para>Returns all elements of this IEnumerable&lt;T&gt; in a single System.String.</para>
        /// <para>Elements are separated by the specified separator.</para>
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">This instance of System.Collections.Generic.IEnumerable&lt;T&gt;.</param>
        /// <param name="separator">The System.String to use to separate each element.</param>
        /// <returns>System.String containing elements from specified IEnumerable&lt;T&gt;.</returns>
        public static string Join<T>(this IEnumerable<T> source, string separator)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (separator == null)
            {
                separator = string.Empty;
            }
            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    return string.Empty;
                }

                var builder = new StringBuilder();
                if (!Equals(enumerator.Current, default(T)))
                {
                    builder.Append(enumerator.Current);
                }

                while (enumerator.MoveNext())
                {
                    builder.Append(separator);
                    if (!Equals(enumerator.Current, default(T)))
                    {
                        builder.Append(enumerator.Current);
                    }
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// <para>Returns all elements of this IEnumerable&lt;T&gt; in a single System.String.</para>
        /// <para>Elements are separated by a comma.</para>
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">This instance of System.Collections.Generic.IEnumerable&lt;T&gt;.</param>
        /// <param name="selector"></param>
        /// <returns>System.String containing elements from specified IEnumerable&lt;T&gt;.</returns>
        public static string Join<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
        {
            return source.Join(selector, ",");
        }

        /// <summary>
        /// <para>Returns all elements of this IEnumerable&lt;T&gt; in a single System.String.</para>
        /// <para>Elements are separated by the specified separator.</para>
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source">This instance of System.Collections.Generic.IEnumerable&lt;T&gt;.</param>
        /// <param name="selector"></param>
        /// <param name="separator"></param>
        /// <returns>System.String containing elements from specified IEnumerable&lt;T&gt;.</returns>
        public static string Join<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector, string separator)
        {
            return source.Select(selector).Join(separator);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static IEnumerable<T> SafeUnion<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first.IsNullOrEmpty())
            {
                return second;
            }
            else if (second.IsNullOrEmpty())
            {
                return first;
            }
            return first.Union(second);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> ToChunks<T>(this IEnumerable<T> source, int size)
        {
            var chunks = new HashSet<HashSet<T>>();
            var chunk = new HashSet<T>();

            int count = 0;
            foreach (var element in source)
            {
                if (count++ == size)
                {
                    chunks.Add(chunk);
                    chunk = new HashSet<T>();
                    count = 1;
                }
                chunk.Add(element);
            }

            chunks.Add(chunk);

            return chunks;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            return source.ToDataTable(string.Concat(typeof(T).Name, "_Table"));
        }

        /// <summary>
        /// Creates and returns a System.Data.DataTable from the specified System.Collections.Generic.IEnumerable&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="enumerable">This instance of System.Collections.Generic.IEnumerable&lt;T&gt;.</param>
        /// <param name="tableName">The value to set for the DataTable's Name property.</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> enumerable, string tableName)
        {
            var table = new DataTable(tableName) { Locale = CultureInfo.InvariantCulture };

            var properties = typeof(T).GetProperties();

            #region If T Is String Or Has No Properties

            if (properties.IsNullOrEmpty() || typeof(T) == typeof(string))
            {
                table.Columns.Add(new DataColumn("Value", typeof(string)));

                foreach (T item in enumerable)
                {
                    DataRow row = table.NewRow();

                    row["Value"] = item.ToString();

                    table.Rows.Add(row);
                }

                return table;
            }

            #endregion If T Is String Or Has No Properties

            #region Else Normal Collection

            foreach (PropertyInfo property in properties)
            {
                table.Columns.Add(new DataColumn(property.Name, property.PropertyType));
            }

            foreach (T item in enumerable)
            {
                DataRow row = table.NewRow();

                foreach (PropertyInfo property in properties)
                {
                    row[property.Name] = property.GetValue(item, null);
                }

                table.Rows.Add(row);
            }

            #endregion Else Normal Collection

            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> ToListOf<T>(this IEnumerable source)
        {
            return (from object item in source select item.ConvertTo<T>()).ToList();
        }

        // http://stackoverflow.com/questions/17971921/how-to-convert-row-to-column-in-linq-and-sql
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <typeparam name="TColumn"></typeparam>
        /// <typeparam name="TRow"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="source"></param>
        /// <param name="columnSelector"></param>
        /// <param name="rowSelector"></param>
        /// <param name="dataSelector"></param>
        /// <returns></returns>
        public static DataTable ToPivotTable<T, TColumn, TRow, TData>(
            this IEnumerable<T> source,
            Func<T, TColumn> columnSelector,
            Expression<Func<T, TRow>> rowSelector,
            Func<IEnumerable<T>, TData> dataSelector)
        {
            var table = new DataTable();
            var rowName = ((MemberExpression)rowSelector.Body).Member.Name;
            table.Columns.Add(new DataColumn(rowName));
            var columns = source.Select(columnSelector).Distinct();

            foreach (var column in columns)
            {
                table.Columns.Add(new DataColumn(column.ToString()));
            }

            var rows = source
                .GroupBy(rowSelector.Compile())
                .Select(rowGroup => new
                {
                    Key = rowGroup.Key,
                    Values = columns.GroupJoin(
                        rowGroup,
                        c => c,
                        r => columnSelector(r),
                        (c, columnGroup) => dataSelector(columnGroup))
                });

            foreach (var row in rows)
            {
                var dataRow = table.NewRow();
                var items = row.Values.Cast<object>().ToList();
                items.Insert(0, row.Key);
                dataRow.ItemArray = items.ToArray();
                table.Rows.Add(dataRow);
            }

            return table;
        }

        /// <summary>
        /// Creates a System.Collections.Generic.Queue&lt;T&gt; from an System.Collections.Generic.IEnumerable&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable&lt;T&gt; to create a System.Collections.Generic.Queue&lt;T&gt; from</param>
        /// <returns>A System.Collections.Generic.Queue&lt;T&gt; that contains elements from the input sequence</returns>
        public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
        {
            var queue = new Queue<T>();
            foreach (T item in source)
            {
                queue.Enqueue(item);
            }
            return queue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IList<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            return new ReadOnlyCollection<T>(source.ToList());
        }

        /// <summary>
        /// Creates a System.Collections.Generic.Stack&lt;T&gt; from an System.Collections.Generic.IEnumerable&lt;T&gt;.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable&lt;T&gt; to create a System.Collections.Generic.Stack&lt;T&gt; from</param>
        /// <returns>A System.Collections.Generic.Stack&lt;T&gt; that contains elements from the input sequence</returns>
        public static Stack<T> ToStack<T>(this IEnumerable<T> source)
        {
            var stack = new Stack<T>();
            foreach (T item in source.Reverse())
            {
                stack.Push(item);
            }
            return stack;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source"></param>
        /// <param name="descendBy"></param>
        /// <returns></returns>
        public static IEnumerable<T> Descendants<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> descendBy)
        {
            foreach (T value in source)
            {
                yield return value;

                foreach (T child in descendBy(value).Descendants<T>(descendBy))
                {
                    yield return child;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToCsv<T>(this IEnumerable<T> source)
        {
            return source.ToCsv(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source"></param>
        /// <param name="outputColumnNames"></param>
        /// <returns></returns>
        public static string ToCsv<T>(this IEnumerable<T> source, bool outputColumnNames)
        {
            var sb = new StringBuilder(2000);

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            #region Column Names

            if (outputColumnNames)
            {
                foreach (PropertyInfo p in properties)
                {
                    sb.Append(p.Name);
                    sb.Append(',');
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(System.Environment.NewLine);
            }

            #endregion Column Names

            #region Rows (Data)

            foreach (var row in source)
            {
                foreach (PropertyInfo p in properties)
                {
                    string value = p.GetValue(row).ToString();

                    sb.Append(value.Contains(",") ? value.AddDoubleQuotes() : value);

                    sb.Append(',');
                }

                //Remove Last ','
                sb.Remove(sb.Length - 1, 1);
                sb.Append(System.Environment.NewLine);
            }

            #endregion Rows (Data)

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool ToCsv<T>(this IEnumerable<T> source, string filePath)
        {
            return source.ToCsv(filePath, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source"></param>
        /// <param name="filePath"></param>
        /// <param name="outputColumnNames"></param>
        /// <returns></returns>
        public static bool ToCsv<T>(this IEnumerable<T> source, string filePath, bool outputColumnNames)
        {
            var sb = new StringBuilder(2000);

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            #region Column Names

            if (outputColumnNames)
            {
                foreach (PropertyInfo p in properties)
                {
                    sb.Append(p.Name);
                    sb.Append(',');
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(System.Environment.NewLine);
            }

            #endregion Column Names

            #region Rows (Data)

            foreach (var row in source)
            {
                foreach (PropertyInfo p in properties)
                {
                    string value = p.GetValue(row).ToString();

                    sb.Append(value.Contains(",") ? value.AddDoubleQuotes() : value);

                    sb.Append(',');
                }

                //Remove Last ','
                sb.Remove(sb.Length - 1, 1);
                sb.Append(System.Environment.NewLine);
            }

            #endregion Rows (Data)

            bool result = sb.ToString().ToFile(filePath);

            return result;
        }

        internal static bool FastAny<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var array = source as TSource[];

            if (array != null)
            {
                return array.Length > 0;
            }

            var collection = source as ICollection<TSource>;

            if (collection != null)
            {
                return collection.Count > 0;
            }

            using (var enumerator = source.GetEnumerator())
            {
                return enumerator.MoveNext();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool HasMoreThan<T>(this IEnumerable<T> source, int n)
        {
            if (source == null)
            {
                return false;
                //throw new ArgumentNullException("source");
            }

            var array = source as T[];

            if (array != null)
            {
                return array.Length > n;
            }

            var collection = source as ICollection<T>;

            if (collection != null)
            {
                return collection.Count > n;
            }

            using (var enumerator = source.GetEnumerator())
            {
                for (int i = 0; i < n + 1; i++)
                {
                    if (!enumerator.MoveNext())
                    {
                        return false;
                    };
                }
                return true;
            }
        }
    }
}
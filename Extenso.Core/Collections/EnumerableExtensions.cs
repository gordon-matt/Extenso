﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Extenso.Collections
{
    /// <summary>
    /// Provides a set of static methods for querying and manipulating objects that implement System.Collections.Generic.IEnumerable`1.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Determines whether a sequence contains any of the specified elements by using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence in which to locate one of the given values.</param>
        /// <param name="values">The values to locate in the sequence.</param>
        /// <returns>true if the source sequence contains an element that has any of the specified values; otherwise, false.</returns>
        public static bool ContainsAny<T>(this IEnumerable<T> source, params T[] values)
        {
            return values.Any(source.Contains);
        }

        /// <summary>
        /// Determines whether a sequence contains any of the specified elements by using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence in which to locate one of the given values.</param>
        /// <param name="values">The values to locate in the sequence.</param>
        /// <returns>true if the source sequence contains an element that has any of the specified values; otherwise, false.</returns>
        public static bool ContainsAny<T>(this IEnumerable<T> source, IEnumerable<T> values)
        {
            return values.Any(source.Contains);
        }

        /// <summary>
        /// Performs the specified action on each element of the System.Collections.Generic.IEnumerable`1.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 containing the elements to perform the specified action on.</param>
        /// <param name="action">The System.Action`1 delegate to perform on each element of the System.Collections.Generic.IEnumerable`1.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Determines whether the sequence contains more than (n) elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The given sequence.</param>
        /// <param name="n">The number of elements to look for.</param>
        /// <returns>true if the number of elements is greater than (n); otherwise false.</returns>
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

        /// <summary>
        /// Indicates whether the specified System.Collections.Generic.IEnumerable`1 is null or empty
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 to evaluate.</param>
        /// <returns>true if the System.Collections.Generic.IEnumerable`1 is null or empty; otherwise, false.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.FastAny();
            //return source == null || !source.Any();
        }

        /// <summary>
        /// Concatenates the members of a sequence, using the specified separator between each member.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence that contains the objects to concatenate.</param>
        /// <param name="separator">The string to use as a separator. separator is included in the returned string only if source has more than one element.</param>
        /// <returns>A string that consists of the members of values delimited by the separator string.</returns>
        public static string Join<T>(this IEnumerable<T> source, string separator = ",")
        {
            return string.Join(separator, source);
        }

        /// <summary>
        /// Concatenates the members of a sequence, using the specified separator between each member.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the result returned by selector.</typeparam>
        /// <param name="source">The sequence that contains the objects to concatenate.</param>
        /// <param name="selector">A function to extract a value from each element.</param>
        /// <param name="separator">The string to use as a separator. separator is included in the returned string only if source has more than one element.</param>
        /// <returns>A string that consists of the members of values returned by selector and delimited by the separator string.</returns>
        public static string Join<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector, string separator = ",")
        {
            return source.Select(selector).Join(separator);
        }

        /// <summary>
        /// Produces the set union of two sequences by using the default equality comparer. If either of the sequences is null or empty, the other sequence is returned.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">The System.Collections.Generic.IEnumerable`1 whose distinct elements form the first set for the union.</param>
        /// <param name="second">The System.Collections.Generic.IEnumerable`1 whose distinct elements form the second set for the union.</param>
        /// <returns>An System.Collections.Generic.IEnumerable`1 that contains the elements from both input sequences, excluding duplicates.</returns>
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
        /// Splits the given collection into chunks of the given size.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The collection which is to be split into chunks.</param>
        /// <param name="size">The number of elements for a single chunk.</param>
        /// <returns>All the elements of source split into chunks of the given size.</returns>
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
        /// Creates and returns a System.Data.DataTable from the elements in the given sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to create a System.Data.DataTable from.</param>
        /// <returns>A new System.Data.DataTable containing columns and rows based on the elements in source.</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            return source.ToDataTable(string.Concat(typeof(T).Name, "_Table"));
        }

        /// <summary>
        /// Creates and returns a System.Data.DataTable from the elements in the given sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The sequence to create a System.Data.DataTable from.</param>
        /// <param name="tableName">The value to set for the System.Data.DataTable's Name property.</param>
        /// <returns>A new System.Data.DataTable containing columns and rows based on the elements in source.</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source, string tableName)
        {
            var table = new DataTable(tableName) { Locale = CultureInfo.InvariantCulture };

            var properties = typeof(T).GetProperties();

            #region If T Is String Or Has No Properties

            if (properties.IsNullOrEmpty() || typeof(T) == typeof(string))
            {
                table.Columns.Add(new DataColumn("Value", typeof(string)));

                foreach (T item in source)
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

            foreach (T item in source)
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
        /// Creates a System.Collections.Generic.HashSet`1 from an System.Collections.Generic.IEnumerable`1.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a System.Collections.Generic.HashSet`1 from.</param>
        /// <returns>A System.Collections.Generic.HashSet`1 that contains elements from the input sequence.</returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        /// <summary>
        /// Creates a System.Collections.Generic.List`1 by converting each element in the specified System.Collections.Generic.IEnumerable`1 to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to convert the elements of source to.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a System.Collections.Generic.List`1 from.</param>
        /// <returns>A System.Collections.Generic.List`1 that contains elements from the input sequence.</returns>
        public static List<T> ToListOf<T>(this IEnumerable source)
        {
            return (from object item in source select item.ConvertTo<T>()).ToList();
        }

        // http://stackoverflow.com/questions/17971921/how-to-convert-row-to-column-in-linq-and-sql
        /// <summary>
        /// Creates a System.Data.DataTable with the columns, rows and data determined by the given selector functions.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <typeparam name="TColumn">The type of the result returned by columnSelector.</typeparam>
        /// <typeparam name="TRow">The type of the result returned by rowSelector.</typeparam>
        /// <typeparam name="TData">The type of the result returned by dataSelector.</typeparam>
        /// <param name="source">The collection from which to create a System.Data.DataTable.</param>
        /// <param name="columnSelector">A function to extract values from the elements which are to determine the columns.</param>
        /// <param name="rowSelector">A function to extract values from the elements which are to determine the rows.</param>
        /// <param name="dataSelector">A function to extract values from the elements which are to determine the data.</param>
        /// <returns>A System.Data.DataTable with the columns, rows and data determined by the given selector functions.</returns>
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
        /// Creates a System.Collections.Generic.Queue`1 from an System.Collections.Generic.IEnumerable`1.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a System.Collections.Generic.Queue`1 from.</param>
        /// <returns>A System.Collections.Generic.Queue`1 that contains elements from the input sequence.</returns>
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
        /// Creates a System.Collections.ObjectModel.ReadOnlyCollection`1 from an System.Collections.Generic.IEnumerable`1.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a System.Collections.ObjectModel.ReadOnlyCollection`1 from.</param>
        /// <returns>A System.Collections.ObjectModel.ReadOnlyCollection`1 that contains elements from the input sequence.</returns>
        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            return new ReadOnlyCollection<T>(source.ToList());
        }

        /// <summary>
        /// Creates a System.Collections.Generic.Stack`1 from an System.Collections.Generic.IEnumerable`1.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a System.Collections.Generic.Stack`1 from.</param>
        /// <returns>A System.Collections.Generic.Stack`1 that contains elements from the input sequence.</returns>
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
        /// Returns a collection of elements that contains the descendant elements of the same type in source.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 to find descendant elements in.</param>
        /// <param name="descendBy">A function to find the child collection to descend by.</param>
        /// <returns>A collection of elements that contains the descendant elements of the same type in source.</returns>
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
        /// Returns a string containing the elements of source in CSV format.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a CSV formatted string from.</param>
        /// <param name="outputColumnNames">Specifies whether to output column names or not.</param>
        /// <returns>A string containing the elements of source in CSV format.</returns>
        public static string ToCsv<T>(this IEnumerable<T> source, bool outputColumnNames = true)
        {
            var sb = new StringBuilder(2000);

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            #region Column Names

            if (outputColumnNames)
            {
                foreach (var propertyInfo in properties)
                {
                    sb.Append(propertyInfo.Name);
                    sb.Append(',');
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);
            }

            #endregion Column Names

            #region Rows (Data)

            foreach (var element in source)
            {
                foreach (var propertyInfo in properties)
                {
                    string value = propertyInfo.GetValue(element).ToString();
                    sb.Append(value.Contains(",") ? value.AddDoubleQuotes() : value);
                    sb.Append(',');
                }

                // Remove trailing comma
                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);
            }

            #endregion Rows (Data)

            return sb.ToString();
        }

        /// <summary>
        /// Writes the elements to file in CSV format.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a CSV formatted file from.</param>
        /// <param name="filePath">The full path of the file to write to.</param>
        /// <param name="outputColumnNames">Specifies whether to output column names or not.</param>
        /// <returns>true if successful; otherwise false.</returns>
        public static bool ToCsv<T>(this IEnumerable<T> source, string filePath, bool outputColumnNames = true)
        {
            return source.ToCsv(outputColumnNames).ToFile(filePath);
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
    }
}
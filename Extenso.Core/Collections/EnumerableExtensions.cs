using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Extenso.Collections;

/// <summary>
/// Provides a set of static methods for querying and manipulating objects that implement System.Collections.Generic.IEnumerable`1.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// If source is already a <see cref="HashSet{T}"/>, then returns as-is. Else, a ToHashSet() is performed.
    /// Simply performing a ToHashSet() will always create a new HashSet.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to return as a <see cref="HashSet{T}"/>.</param>
    /// <returns>A <see cref="HashSet{T}"/> that contains elements from the input sequence.</returns>
    public static HashSet<T> AsHashSet<T>(this IEnumerable<T> source) => source is HashSet<T> ? source as HashSet<T> : source.ToHashSet();

    /// <summary>
    /// If source is already a <see cref="List{T}"/>, then returns as-is. Else, a ToList() is performed.
    /// Simply performing a ToList() will always create a new list.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source</typeparam>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to return as a <see cref="List{T}"/>.</param>
    /// <returns>A <see cref="List{T}"/> that contains elements from the input sequence.</returns>
    public static List<T> AsList<T>(this IEnumerable<T> source) => source is List<T> ? source as List<T> : source.ToList();

    /// <summary>
    /// Determines whether a sequence contains any of the specified elements by using the default equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence in which to locate one of the given values.</param>
    /// <param name="values">The values to locate in the sequence.</param>
    /// <returns>true if the source sequence contains an element that has any of the specified values; otherwise, false.</returns>
    public static bool ContainsAny<T>(this IEnumerable<T> source, params T[] values) =>
        values.Any(source.Contains);

    /// <summary>
    /// Determines whether a sequence contains any of the specified elements by using the default equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence in which to locate one of the given values.</param>
    /// <param name="values">The values to locate in the sequence.</param>
    /// <returns>true if the source sequence contains an element that has any of the specified values; otherwise, false.</returns>
    public static bool ContainsAny<T>(this IEnumerable<T> source, IEnumerable<T> values) =>
        values.Any(source.Contains);

    /// <summary>
    /// Determines whether a sequence contains all of the specified elements by using the default equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence in which to locate all of the given values.</param>
    /// <param name="values">The values to locate in the sequence.</param>
    /// <returns>true if the source sequence contains all of the specified values; otherwise, false.</returns>
    public static bool ContainsAll<T>(this IEnumerable<T> source, params T[] values) =>
        values.All(source.Contains);

    /// <summary>
    /// Determines whether a sequence contains all of the specified elements by using the default equality comparer.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence in which to locate all of the given values.</param>
    /// <param name="values">The values to locate in the sequence.</param>
    /// <returns>true if the source sequence contains all of the specified values; otherwise, false.</returns>
    public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> values) =>
        values.All(source.Contains);

    /// <summary>
    /// Returns a collection of elements that contains the descendant elements of the same type in source.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The System.Collections.Generic.IEnumerable`1 to find descendant elements in.</param>
    /// <param name="descendBy">A function to find the child collection to descend by.</param>
    /// <returns>A collection of elements that contains the descendant elements of the same type in source.</returns>
    public static IEnumerable<T> Descendants<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> descendBy)
    {
        foreach (var value in source)
        {
            yield return value;

            foreach (var child in descendBy(value).Descendants<T>(descendBy))
            {
                yield return child;
            }
        }
    }

    // Copied from https://github.com/morelinq/MoreLINQ/

    /// <summary>
    /// Returns all distinct elements of the given source, where "distinctness"
    /// is determined via a projection and the specified comparer for the projected type.
    /// </summary>
    /// <remarks>
    /// This operator uses deferred execution and streams the results, although
    /// a set of already-seen keys is retained. If a key is seen multiple times,
    /// only the first element with that key is returned.
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="keySelector">Projection for determining "distinctness"</param>
    /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
    /// If null, the default equality comparer for <c>TSource</c> is used.</param>
    /// <returns>A sequence consisting of distinct elements from the source sequence,
    /// comparing them by the specified key projection.</returns>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey> comparer = null)
    {
        return source is null
            ? throw new ArgumentNullException(nameof(source))
            : keySelector is null ? throw new ArgumentNullException(nameof(keySelector)) : _();
        IEnumerable<TSource> _()
        {
            var knownKeys = new HashSet<TKey>(comparer);
            foreach (var element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }

    public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
    {
        var queue = new Queue<T>();
        foreach (var item in source)
        {
            queue.Enqueue(item);
        }

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            yield return current;

            var children = selector(current);
            if (children is null)
            {
                continue;
            }

            foreach (var child in children)
            {
                queue.Enqueue(child);
            }
        }
    }

    /// <summary>
    /// Performs the specified action on each element of the System.Collections.Generic.IEnumerable`1.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The System.Collections.Generic.IEnumerable`1 containing the elements to perform the specified action on.</param>
    /// <param name="action">The System.Action`1 delegate to perform on each element of the System.Collections.Generic.IEnumerable`1.</param>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
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
        if (source is null)
        {
            return false;
            //throw new ArgumentNullException("source");
        }

        if (source is T[] array)
        {
            return array.Length > n;
        }

        if (source is ICollection<T> collection)
        {
            return collection.Count > n;
        }

        using var enumerator = source.GetEnumerator();
        for (int i = 0; i < n + 1; i++)
        {
            if (!enumerator.MoveNext())
            {
                return false;
            }
            ;
        }
        return true;
    }

    /// <summary>
    /// Indicates whether the specified System.Collections.Generic.IEnumerable`1 is null or empty
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The System.Collections.Generic.IEnumerable`1 to evaluate.</param>
    /// <returns>true if the System.Collections.Generic.IEnumerable`1 is null or empty; otherwise, false.</returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) =>
        source is null || !source.FastAny();

    /// <summary>
    /// Concatenates the members of a sequence, using the specified separator between each member.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence that contains the objects to concatenate.</param>
    /// <param name="separator">The string to use as a separator. separator is included in the returned string only if source has more than one element.</param>
    /// <returns>A string that consists of the members of values delimited by the separator string.</returns>
    public static string Join<T>(this IEnumerable<T> source, string separator = ",") =>
        string.Join(separator, source);

    /// <summary>
    /// Concatenates the members of a sequence, using the specified separator between each member.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by selector.</typeparam>
    /// <param name="source">The sequence that contains the objects to concatenate.</param>
    /// <param name="selector">A function to extract a value from each element.</param>
    /// <param name="separator">The string to use as a separator. separator is included in the returned string only if source has more than one element.</param>
    /// <returns>A string that consists of the members of values returned by selector and delimited by the separator string.</returns>
    public static string Join<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector, string separator = ",") =>
        source.Select(selector).Join(separator);

    /// <summary>
    /// Returns the most occurring element in the given collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <typeparam name="TProp"></typeparam>
    /// <param name="source">The sequence that contains the objects to examine.</param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static TProp MostOccurring<T, TProp>(this IEnumerable<T> source, Func<T, TProp> selector) =>
        source.GroupBy(selector).OrderByDescending(x => x.Count()).First().Key;

    /// <summary>
    /// Returns the most occurring element in the given collection or default if the collection is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <typeparam name="TProp"></typeparam>
    /// <param name="source">The sequence that contains the objects to examine.</param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static TProp MostOccurringOrDefault<T, TProp>(this IEnumerable<T> source, Func<T, TProp> selector) =>
        source.IsNullOrEmpty() ? default : source.MostOccurring(selector);

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
        if (source.IsNullOrEmpty())
            return []; // Return empty if source is empty

        var chunks = new HashSet<HashSet<T>>();
        var chunk = new HashSet<T>();

        int count = 0;
        foreach (var element in source)
        {
            if (count++ == size)
            {
                chunks.Add(chunk);
                chunk = [];
                count = 1;
            }
            chunk.Add(element);
        }

        chunks.Add(chunk);

        return chunks;
    }

    /// <summary>
    /// Returns a string containing the elements of source in CSV format.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a CSV formatted string from.</param>
    /// <param name="outputColumnNames">Specifies whether to output column names or not.</param>
    /// <param name="alwaysEnquote"></param>
    /// <returns>A string containing the elements of source in CSV format.</returns>
    public static string ToCsv<T>(this IEnumerable<T> source, bool outputColumnNames = true, bool alwaysEnquote = true) =>
        ToDelimited(source, ",", outputColumnNames, alwaysEnquote);

    /// <summary>
    /// Writes the elements to file in CSV format.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a CSV formatted file from.</param>
    /// <param name="filePath">The full path of the file to write to.</param>
    /// <param name="outputColumnNames">Specifies whether to output column names or not.</param>
    /// <param name="alwaysEnquote"></param>
    /// <returns>true if successful; otherwise false.</returns>
    public static bool ToCsv<T>(this IEnumerable<T> source, string filePath, bool outputColumnNames = true, bool alwaysEnquote = true) =>
        ToDelimited(source, ",", filePath, outputColumnNames, alwaysEnquote);

    /// <summary>
    /// Returns a delimited string containing the elements of source.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a delimited string from.</param>
    /// <param name="delimiter">The character(s) used to separate the property values of each element in source.</param>
    /// <param name="outputColumnNames">Specifies whether to output column names or not.</param>
    /// <param name="alwaysEnquote"></param>
    /// <returns>A delimited string containing the elements of source.</returns>
    public static string ToDelimited<T>(this IEnumerable<T> source, string delimiter = ",", bool outputColumnNames = true, bool alwaysEnquote = true)
    {
        var sb = new StringBuilder(2000);

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        #region Column Names

        if (outputColumnNames)
        {
            foreach (var propertyInfo in properties)
            {
                sb.Append(propertyInfo.Name);
                sb.Append(delimiter);
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
                string value = propertyInfo.GetValue(element).ToString().Replace("\"", "\"\"");

                if (alwaysEnquote || value.Contains(delimiter))
                {
                    sb.Append(value.EnquoteDouble());
                }
                else
                {
                    value = Regex.Replace(value, @"\r\n", " ");
                    value = Regex.Replace(value, @"\t|\r|\n", " ");
                    sb.Append(value);
                }

                sb.Append(delimiter);
            }

            // Remove trailing comma
            sb.Remove(sb.Length - 1, 1);
            sb.Append(Environment.NewLine);
        }

        #endregion Rows (Data)

        return sb.ToString();
    }

    /// <summary>
    /// Writes the elements to file using the specified delimiter.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a delimited file from.</param>
    /// <param name="delimiter">The character(s) used to separate the property values of each element in source.</param>
    /// <param name="filePath">The full path of the file to write to.</param>
    /// <param name="outputColumnNames">Specifies whether to output column names or not.</param>
    /// <param name="alwaysEnquote"></param>
    /// <returns>true if successful; otherwise false.</returns>
    public static bool ToDelimited<T>(
        this IEnumerable<T> source,
        string filePath,
        string delimiter = ",",
        bool outputColumnNames = true,
        bool alwaysEnquote = true) =>
        source.ToDelimited(delimiter, outputColumnNames, alwaysEnquote).ToFile(filePath);

    /// <summary>
    /// Creates and returns a System.Data.DataTable from the elements in the given sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to create a System.Data.DataTable from.</param>
    /// <param name="convertNullableTypes"></param>
    /// <returns>A new System.Data.DataTable containing columns and rows based on the elements in source.</returns>
    public static DataTable ToDataTable<T>(this IEnumerable<T> source, bool convertNullableTypes = false) =>
        source.ToDataTable(string.Concat(typeof(T).Name, "_Table"), convertNullableTypes);

    /// <summary>
    /// Creates and returns a System.Data.DataTable from the elements in the given sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The sequence to create a System.Data.DataTable from.</param>
    /// <param name="tableName">The value to set for the System.Data.DataTable's Name property.</param>
    /// <param name="convertNullableTypes">If true, will convert nullable types to their underlying types. Useful for cases like SQL bulk insert.</param>
    /// <returns>A new System.Data.DataTable containing columns and rows based on the elements in source.</returns>
    public static DataTable ToDataTable<T>(this IEnumerable<T> source, string tableName, bool convertNullableTypes = false)
    {
        var table = new DataTable(tableName) { Locale = CultureInfo.InvariantCulture };

        var properties = typeof(T).GetProperties();

        #region If T Is String Or Has No Properties

        if (properties.IsNullOrEmpty() || typeof(T) == typeof(string))
        {
            table.Columns.Add(new DataColumn("Value", typeof(string)));

            foreach (var item in source)
            {
                var row = table.NewRow();
                row["Value"] = item.ToString();
                table.Rows.Add(row);
            }

            return table;
        }

        #endregion If T Is String Or Has No Properties

        #region Else Normal Collection

        foreach (var property in properties)
        {
            if (convertNullableTypes)
            {
                table.Columns.Add(new DataColumn(
                    property.Name,
                    Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType));
            }
            else
            {
                table.Columns.Add(new DataColumn(property.Name, property.PropertyType));
            }
        }

        foreach (var item in source)
        {
            var row = table.NewRow();

            foreach (var property in properties)
            {
                row[property.Name] = convertNullableTypes ? property.GetValue(item, null) ?? DBNull.Value : property.GetValue(item, null);
            }

            table.Rows.Add(row);
        }

        #endregion Else Normal Collection

        return table;
    }

    /// <summary>
    /// Creates a System.Collections.Generic.List`1 by converting each element in the specified System.Collections.Generic.IEnumerable`1 to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to convert the elements of source to.</typeparam>
    /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a System.Collections.Generic.List`1 from.</param>
    /// <returns>A System.Collections.Generic.List`1 that contains elements from the input sequence.</returns>
    public static List<T> ToListOf<T>(this IEnumerable source) =>
        (from object item in source select item.ConvertTo<T>()).ToList();

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
        string rowName = ((MemberExpression)rowSelector.Body).Member.Name;
        table.Columns.Add(new DataColumn(rowName, typeof(TRow)));
        var columns = source.Select(columnSelector).Distinct();

        foreach (var column in columns)
        {
            table.Columns.Add(new DataColumn(column.ToString(), typeof(TData)));
        }

        var rows = source
            .GroupBy(rowSelector.Compile())
            .Select(rowGroup => new
            {
                rowGroup.Key,
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
    /// Creates a System.Data.DataTable with the columns, rows and data determined by the given selector functions.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <typeparam name="TColumn">The type of the result returned by columnSelector.</typeparam>
    /// <typeparam name="TRow1">The type of the result returned by row1Selector.</typeparam>
    /// <typeparam name="TRow2">The type of the result returned by row2Selector.</typeparam>
    /// <typeparam name="TData">The type of the result returned by dataSelector.</typeparam>
    /// <param name="source">The collection from which to create a System.Data.DataTable.</param>
    /// <param name="columnSelector">A function to extract values from the elements which are to determine the columns.</param>
    /// <param name="row1Selector">A function to extract values from the elements which are to determine the first set of rows.</param>
    /// <param name="row2Selector">A function to extract values from the elements which are to determine the second set of rows.</param>
    /// <param name="dataSelector">A function to extract values from the elements which are to determine the data.</param>
    /// <returns>A System.Data.DataTable with the columns, rows and data determined by the given selector functions.</returns>
    public static DataTable ToPivotTable<T, TColumn, TRow1, TRow2, TData>(
        this IEnumerable<T> source,
        Func<T, TColumn> columnSelector,
        Expression<Func<T, TRow1>> row1Selector,
        Expression<Func<T, TRow2>> row2Selector,
        Func<IEnumerable<T>, TData> dataSelector)
    {
        var table = new DataTable();

        string row1Name = ((MemberExpression)row1Selector.Body).Member.Name;
        table.Columns.Add(new DataColumn(row1Name, typeof(TRow1)));

        string row2Name = ((MemberExpression)row2Selector.Body).Member.Name;
        table.Columns.Add(new DataColumn(row2Name, typeof(TRow2)));

        var columns = source.Select(columnSelector).Distinct();

        foreach (var column in columns)
        {
            table.Columns.Add(new DataColumn(column.ToString(), typeof(TData)));
        }

        var rows = source
            .GroupBy(x => new { Row1Value = row1Selector.Compile()(x), Row2Value = row2Selector.Compile()(x) })
            .Select(rowGroup => new
            {
                rowGroup.Key.Row1Value,
                rowGroup.Key.Row2Value,
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
            items.Insert(0, row.Row2Value);
            items.Insert(0, row.Row1Value);
            dataRow.ItemArray = items.ToArray();
            table.Rows.Add(dataRow);
        }

        return table;
    }

    /// <summary>
    /// Creates a System.Data.DataTable with the columns, rows and data determined by the given selector functions.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <typeparam name="TColumn">The type of the result returned by columnSelector.</typeparam>
    /// <typeparam name="TRow1">The type of the result returned by row1Selector.</typeparam>
    /// <typeparam name="TRow2">The type of the result returned by row2Selector.</typeparam>
    /// <typeparam name="TRow3">The type of the result returned by row3Selector.</typeparam>
    /// <typeparam name="TData">The type of the result returned by dataSelector.</typeparam>
    /// <param name="source">The collection from which to create a System.Data.DataTable.</param>
    /// <param name="columnSelector">A function to extract values from the elements which are to determine the columns.</param>
    /// <param name="row1Selector">A function to extract values from the elements which are to determine the first set of rows.</param>
    /// <param name="row2Selector">A function to extract values from the elements which are to determine the second set of rows.</param>
    /// <param name="row3Selector">A function to extract values from the elements which are to determine the third set of rows.</param>
    /// <param name="dataSelector">A function to extract values from the elements which are to determine the data.</param>
    /// <returns>A System.Data.DataTable with the columns, rows and data determined by the given selector functions.</returns>
    public static DataTable ToPivotTable<T, TColumn, TRow1, TRow2, TRow3, TData>(
        this IEnumerable<T> source,
        Func<T, TColumn> columnSelector,
        Expression<Func<T, TRow1>> row1Selector,
        Expression<Func<T, TRow2>> row2Selector,
        Expression<Func<T, TRow3>> row3Selector,
        Func<IEnumerable<T>, TData> dataSelector)
    {
        var table = new DataTable();

        string row1Name = ((MemberExpression)row1Selector.Body).Member.Name;
        table.Columns.Add(new DataColumn(row1Name, typeof(TRow1)));

        string row2Name = ((MemberExpression)row2Selector.Body).Member.Name;
        table.Columns.Add(new DataColumn(row2Name, typeof(TRow2)));

        string row3Name = ((MemberExpression)row3Selector.Body).Member.Name;
        table.Columns.Add(new DataColumn(row3Name, typeof(TRow3)));

        var columns = source.Select(columnSelector).Distinct();

        foreach (var column in columns)
        {
            table.Columns.Add(new DataColumn(column.ToString(), typeof(TData)));
        }

        var rows = source
            .GroupBy(x => new
            {
                Row1Value = row1Selector.Compile()(x),
                Row2Value = row2Selector.Compile()(x),
                Row3Value = row3Selector.Compile()(x)
            })
            .Select(rowGroup => new
            {
                rowGroup.Key.Row1Value,
                rowGroup.Key.Row2Value,
                rowGroup.Key.Row3Value,
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
            items.Insert(0, row.Row3Value);
            items.Insert(0, row.Row2Value);
            items.Insert(0, row.Row1Value);
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
        foreach (var item in source)
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
    public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source) =>
        new(source.ToList());

    /// <summary>
    /// Creates a System.Collections.Generic.Stack`1 from an System.Collections.Generic.IEnumerable`1.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">The System.Collections.Generic.IEnumerable`1 to create a System.Collections.Generic.Stack`1 from.</param>
    /// <returns>A System.Collections.Generic.Stack`1 that contains elements from the input sequence.</returns>
    public static Stack<T> ToStack<T>(this IEnumerable<T> source)
    {
        var stack = new Stack<T>();
        foreach (var item in source.Reverse())
        {
            stack.Push(item);
        }
        return stack;
    }

    internal static bool FastAny<TSource>(this IEnumerable<TSource> source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (source is TSource[] array)
        {
            return array.Length > 0;
        }

        if (source is ICollection<TSource> collection)
        {
            return collection.Count > 0;
        }

        using var enumerator = source.GetEnumerator();
        return enumerator.MoveNext();
    }
}
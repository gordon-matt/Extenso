using System.Collections.Generic;
using System.Data;

namespace Extenso.Data
{
    public static class DataColumnCollectionExtensions
    {
        public static void AddRange(this DataColumnCollection dataColumns, params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                dataColumns.Add(columnName);
            }
        }

        public static void AddRange(this DataColumnCollection dataColumns, params DataColumn[] columns)
        {
            foreach (DataColumn column in columns)
            {
                dataColumns.Add(column);
            }
        }

        public static void AddRange<T>(this DataColumnCollection dataColumns, IEnumerable<string> enumerable)
        {
            foreach (string item in enumerable)
            {
                dataColumns.Add(item);
            }
        }
    }
}
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
            foreach (var column in columns)
            {
                dataColumns.Add(column);
            }
        }
    }
}
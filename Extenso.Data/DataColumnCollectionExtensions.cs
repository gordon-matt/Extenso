using System.Data;

namespace Extenso.Data;

public static class DataColumnCollectionExtensions
{
    extension(DataColumnCollection dataColumns)
    {
        public void AddRange(params ReadOnlySpan<string> columnNames)
        {
            foreach (string columnName in columnNames)
            {
                dataColumns.Add(columnName);
            }
        }

        public void AddRange(params ReadOnlySpan<DataColumn> columns)
        {
            foreach (var column in columns)
            {
                dataColumns.Add(column);
            }
        }
    }
}
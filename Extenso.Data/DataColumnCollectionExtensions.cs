using System.Data;

namespace Extenso.Data;

public static class DataColumnCollectionExtensions
{
    public static void AddRange(this DataColumnCollection dataColumns, params ReadOnlySpan<string> columnNames)
    {
        foreach (string columnName in columnNames)
        {
            dataColumns.Add(columnName);
        }
    }

    public static void AddRange(this DataColumnCollection dataColumns, params ReadOnlySpan<DataColumn> columns)
    {
        foreach (var column in columns)
        {
            dataColumns.Add(column);
        }
    }
}
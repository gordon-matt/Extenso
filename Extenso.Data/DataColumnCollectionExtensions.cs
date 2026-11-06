using System.Data;

namespace Extenso.Data;

/// <summary>
/// Provides extension methods for data column collections.
/// </summary>
public static class DataColumnCollectionExtensions
{
    extension(DataColumnCollection dataColumns)
    {
        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="columnNames">The column names.</param>
        public void AddRange(params ReadOnlySpan<string> columnNames)
        {
            foreach (string columnName in columnNames)
            {
                dataColumns.Add(columnName);
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="columns">The columns.</param>
        public void AddRange(params ReadOnlySpan<DataColumn> columns)
        {
            foreach (var column in columns)
            {
                dataColumns.Add(column);
            }
        }
    }
}
using System.Data;

namespace Extenso.Data;

/// <summary>
/// Provides extension methods for data sets.
/// </summary>
public static class DataSetExtensions
{
    extension(DataSet dataSet)
    {
        /// <summary>
        /// Converts the value to the delimited.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="outputColumnNames">The output column names.</param>
        /// <param name="alwaysEnquote">The always enquote.</param>
        /// <returns>The delimited.</returns>
        public IDictionary<int, string> ToDelimited(string delimiter = ",", bool outputColumnNames = true, bool alwaysEnquote = true)
        {
            var results = new Dictionary<int, string>();

            for (int i = 0; i < dataSet.Tables.Count; i++)
            {
                var table = dataSet.Tables[i];
                string csv = table.ToDelimited(
                    delimiter,
                    outputColumnNames: outputColumnNames,
                    alwaysEnquote: alwaysEnquote);
                results.Add(i, csv);
            }

            return results;
        }

        /// <summary>
        /// Converts the value to the delimited.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="outputColumnNames">The output column names.</param>
        /// <param name="alwaysEnquote">The always enquote.</param>
        public void ToDelimited(string directoryPath, string delimiter = ",", bool outputColumnNames = true, bool alwaysEnquote = true)
        {
            string tableName;
            int tableCount = 0;

            foreach (DataTable table in dataSet.Tables)
            {
                tableName = !string.IsNullOrEmpty(table.TableName) ? table.TableName : $"Table_{tableCount++}";

                string filePath = Path.Combine(directoryPath, $"{tableName}.csv");
                table.ToDelimited(
                    filePath,
                    delimiter,
                    outputColumnNames: outputColumnNames,
                    alwaysEnquote: alwaysEnquote);
            }
        }
    }
}
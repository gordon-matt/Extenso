using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Extenso.Data;

/// <summary>
/// Provides extension methods for data tables.
/// </summary>
public static class DataTableExtensions
{
    extension(DataTable table)
    {
        /// <summary>
        /// Computes the average.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="columnName">The column name.</param>
        /// <returns>The average.</returns>
        public T ComputeAverage<T>(string columnName) => table.Compute<T>(columnName, "AVG");

        /// <summary>
        /// Computes the count.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="columnName">The column name.</param>
        /// <returns>The count.</returns>
        public T ComputeCount<T>(string columnName) => table.Compute<T>(columnName, "COUNT");

        /// <summary>
        /// Computes the max.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="columnName">The column name.</param>
        /// <returns>The max.</returns>
        public T ComputeMax<T>(string columnName) => table.Compute<T>(columnName, "MAX");

        /// <summary>
        /// Computes the min.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="columnName">The column name.</param>
        /// <returns>The min.</returns>
        public T ComputeMin<T>(string columnName) => table.Compute<T>(columnName, "MIN");

        /// <summary>
        /// Computes the sum.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="columnName">The column name.</param>
        /// <returns>The sum.</returns>
        public T ComputeSum<T>(string columnName) => table.Compute<T>(columnName, "SUM");

        /// <summary>
        /// Computes the value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="columnName">The column name.</param>
        /// <param name="functionName">The function name.</param>
        /// <returns>The t.</returns>
        public T Compute<T>(string columnName, string functionName) =>
            (T)Convert.ChangeType(table.Compute(string.Format("{1}({0})", columnName, functionName), string.Empty), typeof(T));

        /// <summary>
        /// Converts the value to the CSV.
        /// </summary>
        /// <param name="outputColumnNames">The output column names.</param>
        /// <param name="alwaysEnquote">The always enquote.</param>
        /// <returns>The CSV.</returns>
        public string ToCsv(bool outputColumnNames = true, bool alwaysEnquote = true) =>
            ToDelimited(table, ",", outputColumnNames, alwaysEnquote);

        /// <summary>
        /// Converts the value to the CSV.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="outputColumnNames">The output column names.</param>
        /// <param name="alwaysEnquote">The always enquote.</param>
        /// <returns>true when the condition is met; otherwise, false.</returns>
        public bool ToCsv(string filePath, bool outputColumnNames = true, bool alwaysEnquote = true) =>
            ToDelimited(table, filePath, ",", outputColumnNames, alwaysEnquote);

        /// <summary>
        /// Converts the value to the delimited.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="outputColumnNames">The output column names.</param>
        /// <param name="alwaysEnquote">The always enquote.</param>
        /// <returns>The delimited.</returns>
        public string ToDelimited(string delimiter = ",", bool outputColumnNames = true, bool alwaysEnquote = true)
        {
            var sb = new StringBuilder(2000);

            #region Column Names

            if (outputColumnNames)
            {
                foreach (DataColumn column in table.Columns)
                {
                    if (alwaysEnquote || column.ColumnName.Contains(delimiter))
                    {
                        sb.Append(column.ColumnName.EnquoteDouble());
                    }
                    else
                    {
                        sb.Append(column.ColumnName);
                    }

                    sb.Append(delimiter);
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);
            }

            #endregion Column Names

            #region Rows (Data)

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    string value = row[column].ToString().Replace("\"", "\"\"");

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

                //Remove Last ','
                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);
            }

            #endregion Rows (Data)

            return sb.ToString();
        }

        /// <summary>
        /// Converts the value to the delimited.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="outputColumnNames">The output column names.</param>
        /// <param name="alwaysEnquote">The always enquote.</param>
        /// <returns>true when the condition is met; otherwise, false.</returns>
        public bool ToDelimited(string filePath, string delimiter = ",", bool outputColumnNames = true, bool alwaysEnquote = true) =>
            table.ToDelimited(delimiter, outputColumnNames, alwaysEnquote).ToFile(filePath);
    }
}
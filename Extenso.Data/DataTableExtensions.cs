using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Extenso.Data;

public static class DataTableExtensions
{
    extension(DataTable table)
    {
        public T ComputeAverage<T>(string columnName) => table.Compute<T>(columnName, "AVG");

        public T ComputeCount<T>(string columnName) => table.Compute<T>(columnName, "COUNT");

        public T ComputeMax<T>(string columnName) => table.Compute<T>(columnName, "MAX");

        public T ComputeMin<T>(string columnName) => table.Compute<T>(columnName, "MIN");

        public T ComputeSum<T>(string columnName) => table.Compute<T>(columnName, "SUM");

        public T Compute<T>(string columnName, string functionName) =>
            (T)Convert.ChangeType(table.Compute(string.Format("{1}({0})", columnName, functionName), string.Empty), typeof(T));

        public string ToCsv(bool outputColumnNames = true, bool alwaysEnquote = true) =>
            ToDelimited(table, ",", outputColumnNames, alwaysEnquote);

        public bool ToCsv(string filePath, bool outputColumnNames = true, bool alwaysEnquote = true) =>
            ToDelimited(table, filePath, ",", outputColumnNames, alwaysEnquote);

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

        public bool ToDelimited(string filePath, string delimiter = ",", bool outputColumnNames = true, bool alwaysEnquote = true) =>
            table.ToDelimited(delimiter, outputColumnNames, alwaysEnquote).ToFile(filePath);
    }
}
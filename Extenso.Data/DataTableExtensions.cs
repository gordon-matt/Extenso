﻿using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Extenso.Data
{
    public static class DataTableExtensions
    {
        public static T ComputeAverage<T>(this DataTable table, string columnName)
        {
            return table.Compute<T>(columnName, "AVG");
        }

        public static T ComputeCount<T>(this DataTable table, string columnName)
        {
            return table.Compute<T>(columnName, "COUNT");
        }

        public static T ComputeMax<T>(this DataTable table, string columnName)
        {
            return table.Compute<T>(columnName, "MAX");
        }

        public static T ComputeMin<T>(this DataTable table, string columnName)
        {
            return table.Compute<T>(columnName, "MIN");
        }

        public static T ComputeSum<T>(this DataTable table, string columnName)
        {
            return table.Compute<T>(columnName, "SUM");
        }

        public static T Compute<T>(this DataTable table, string columnName, string functionName)
        {
            return (T)Convert.ChangeType(table.Compute(string.Format("{1}({0})", columnName, functionName), string.Empty), typeof(T));
        }

        public static string ToCsv(this DataTable table, bool outputColumnNames = true, bool alwaysEnquote = true)
        {
            return ToDelimited(table, ",", outputColumnNames, alwaysEnquote);
        }

        public static bool ToCsv(this DataTable table, string filePath, bool outputColumnNames = true, bool alwaysEnquote = true)
        {
            return ToDelimited(table, filePath, ",", outputColumnNames, alwaysEnquote);
        }

        public static string ToDelimited(this DataTable table, string delimiter = ",", bool outputColumnNames = true, bool alwaysEnquote = true)
        {
            var sb = new StringBuilder(2000);

            #region Column Names

            if (outputColumnNames)
            {
                foreach (DataColumn column in table.Columns)
                {
                    sb.Append(column.ColumnName);
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

        public static bool ToDelimited(this DataTable table, string filePath, string delimiter = ",", bool outputColumnNames = true, bool alwaysEnquote = true)
        {
            return table.ToDelimited(delimiter, outputColumnNames, alwaysEnquote).ToFile(filePath);
        }
    }
}
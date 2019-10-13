using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Extenso.Data
{
    public static class DataSetExtensions
    {
        public static IDictionary<int, string> ToDelimited(this DataSet dataSet, string delimiter = ",", bool outputColumnNames = true)
        {
            var results = new Dictionary<int, string>();

            for (int i = 0; i < dataSet.Tables.Count; i++)
            {
                var table = dataSet.Tables[i];
                string csv = table.ToDelimited(delimiter, outputColumnNames);
                results.Add(i, csv);
            }

            return results;
        }

        public static void ToDelimited(this DataSet dataSet, string directoryPath, string delimiter = ",", bool outputColumnNames = true)
        {
            string tableName = string.Empty;
            int tableCount = 0;

            foreach (DataTable table in dataSet.Tables)
            {
                if (!string.IsNullOrEmpty(table.TableName))
                {
                    tableName = table.TableName;
                }
                else
                {
                    tableName = $"Table_{tableCount++}";
                }

                string filePath = Path.Combine(directoryPath, $"{tableName}.csv");
                table.ToDelimited(filePath, delimiter, outputColumnNames);
            }
        }
    }
}
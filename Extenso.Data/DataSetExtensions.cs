using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Extenso.Data
{
    public static class DataSetExtensions
    {
        public static IDictionary<int, string> ToCsv(this DataSet dataSet)
        {
            return dataSet.ToCsv(true);
        }

        public static IDictionary<int, string> ToCsv(this DataSet dataSet, bool outputColumnNames)
        {
            var results = new Dictionary<int, string>();

            for (int i = 0; i < dataSet.Tables.Count; i++)
            {
                var table = dataSet.Tables[i];
                string csv = table.ToCsv(outputColumnNames);
                results.Add(i, csv);
            }

            return results;
        }

        public static void ToCsv(this DataSet dataSet, string directoryPath)
        {
            dataSet.ToCsv(directoryPath, true);
        }

        public static void ToCsv(this DataSet dataSet, string directoryPath, bool outputColumnNames)
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
                    tableName = string.Concat("Table", tableCount++);
                }

                table.ToCsv(Path.Combine(directoryPath, string.Concat(tableName, ".csv")), outputColumnNames);
            }
        }
    }
}
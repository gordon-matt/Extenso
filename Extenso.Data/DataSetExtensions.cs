using System.Data;
using System.IO;

namespace Extenso.Data
{
    public static class DataSetExtensions
    {
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
                { tableName = string.Concat("Table", tableCount++); }

                table.ToCsv(Path.Combine(directoryPath, string.Concat(tableName, ".csv")), outputColumnNames);
            }
        }
    }
}
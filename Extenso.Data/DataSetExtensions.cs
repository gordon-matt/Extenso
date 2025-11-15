using System.Data;

namespace Extenso.Data;

public static class DataSetExtensions
{
    extension(DataSet dataSet)
    {
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
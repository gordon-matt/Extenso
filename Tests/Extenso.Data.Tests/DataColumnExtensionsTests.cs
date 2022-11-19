using System.Data;

namespace Extenso.Data.Tests
{
    public class DataColumnExtensionsTests
    {
        [Fact]
        public void ChangeDataType()
        {
            var table = new DataTable();
            var column = table.Columns.Add("Col1", typeof(string));

            var row = table.NewRow();
            row.SetField("Col1", "100");
            table.Rows.Add(row);

            row = table.NewRow();
            row.SetField("Col1", "200");
            table.Rows.Add(row);

            row = table.NewRow();
            row.SetField("Col1", "300");
            table.Rows.Add(row);

            column.ChangeDataType<int>();

            // Need to reference it again, due to the way ChangeDataType() works
            column = table.Columns.OfType<DataColumn>().FirstOrDefault(x => x.ColumnName == "Col1");

            Assert.True(column.DataType == typeof(int));

            int val = table.Rows[2].Field<int>("Col1");
            Assert.Equal(300, val);
        }

        [Fact]
        public void ChangeDataType_WithFunc()
        {
            var table = new DataTable();
            var column = table.Columns.Add("Col1", typeof(string));

            var row = table.NewRow();
            row.SetField("Col1", "100");
            table.Rows.Add(row);

            row = table.NewRow();
            row.SetField("Col1", "200");
            table.Rows.Add(row);

            row = table.NewRow();
            row.SetField("Col1", "300");
            table.Rows.Add(row);

            column.ChangeDataType<string, int>(x => Convert.ToInt32(x) * 3);

            // Need to reference it again, due to the way ChangeDataType() works
            column = table.Columns.OfType<DataColumn>().FirstOrDefault(x => x.ColumnName == "Col1");

            Assert.True(column.DataType == typeof(int));

            int val = table.Rows[2].Field<int>("Col1");
            Assert.Equal(900, val);
        }

        [Fact]
        public void ColumnLength()
        {
            var table = new DataTable();
            var column = table.Columns.Add("Col1", typeof(string));

            string val1 = "the quick brown fox";
            string val2 = "jumps over the lazy dog";
            string val3 = "fee fi fo fum";

            int expected = val2.Length;

            var row = table.NewRow();
            row.SetField("Col1", val1);
            table.Rows.Add(row);

            row = table.NewRow();
            row.SetField("Col1", val2);
            table.Rows.Add(row);

            row = table.NewRow();
            row.SetField("Col1", val3);
            table.Rows.Add(row);

            int actual = column.ColumnLength();
            Assert.Equal(expected, actual);
        }
    }
}
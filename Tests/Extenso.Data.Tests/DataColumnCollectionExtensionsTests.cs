using System.Data;

namespace Extenso.Data.Tests;

public class DataColumnCollectionExtensionsTests
{
    [Fact]
    public void AddRange_StringParams()
    {
        var table = new DataTable();
        table.Columns.AddRange("Col1", "Col2", "Col3");
        Assert.Contains(table.Columns.OfType<DataColumn>(), x => x.ColumnName == "Col1");
        Assert.Contains(table.Columns.OfType<DataColumn>(), x => x.ColumnName == "Col2");
        Assert.Contains(table.Columns.OfType<DataColumn>(), x => x.ColumnName == "Col3");
    }

    [Fact]
    public void AddRange_DataColumnParams()
    {
        var table = new DataTable();
        table.Columns.AddRange(
            new DataColumn { ColumnName = "Col1" },
            new DataColumn { ColumnName = "Col2" },
            new DataColumn { ColumnName = "Col3" });

        Assert.Contains(table.Columns.OfType<DataColumn>(), x => x.ColumnName == "Col1");
        Assert.Contains(table.Columns.OfType<DataColumn>(), x => x.ColumnName == "Col2");
        Assert.Contains(table.Columns.OfType<DataColumn>(), x => x.ColumnName == "Col3");
    }
}
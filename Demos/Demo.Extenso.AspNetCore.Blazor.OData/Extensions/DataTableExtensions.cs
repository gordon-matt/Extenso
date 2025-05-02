using System.Data;
using Extenso;
using OfficeOpenXml;

namespace Demo.Extenso.AspNetCore.Blazor.OData.Extensions;

public static class DataTableExtensions
{
    public static byte[] ToXlsx(this DataTable table) => table.ToXlsx(true);

    public static byte[] ToXlsx(this DataTable table, bool outputColumnNames)
    {
        using var excel = new ExcelPackage();

        string sheetName = string.IsNullOrEmpty(table.TableName)
            ? "Report"
            : table.TableName;

        var worksheet = excel.Workbook.Worksheets.Add(sheetName);
        worksheet.Cells["A1"].LoadFromDataTable(table, outputColumnNames);

        int columnNumber = 1;
        foreach (DataColumn column in table.Columns)
        {
            if (column.DataType == typeof(DateTime))
            {
                worksheet.Column(columnNumber).Style.Numberformat.Format = "yyyy-mm-dd hh:mm";
            }
            else if (column.DataType == typeof(string))
            {
                if (table.Rows.OfType<DataRow>().Any(x => !x.IsNull(column) && x.Field<string>(column).ContainsAny("\n", "\r\n")))
                {
                    worksheet.Column(columnNumber).Style.WrapText = true;
                }
            }
            columnNumber++;
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        return excel.GetAsByteArray();
    }
}
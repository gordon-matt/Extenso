using System.Data;
using Extenso.Reflection;

namespace Extenso.Data;

public static class DataColumnExtensions
{
    public static bool ChangeDataType<T>(this DataColumn column)
    {
        string temp = "_1_TEMP_1_";

        var newColumn = new DataColumn(column.ColumnName + temp, typeof(T));
        column.Table.Columns.Add(newColumn);

        if (column.Table.Rows.Count > 0)
        {
            if (typeof(T) == typeof(string))
            {
                bool isEnum = column.DataType.IsEnum;

                foreach (DataRow row in column.Table.Rows)
                {
                    row[newColumn] = isEnum ? Enum.ToObject(column.DataType, Convert.ToInt32(row[column])).ToString() : row[column].ToString();
                }

                //Delete original column
                column.Table.Columns.Remove(column);
                newColumn.ColumnName = newColumn.ColumnName.Replace(temp, string.Empty);
            }
            else
            {
                bool success = column.Table.Rows[0][column].ToString().TryParseOrDefault(out T test);

                if (success)
                {
                    foreach (DataRow row in column.Table.Rows)
                    {
                        if (row[column].ToString().TryParseOrDefault(out T value))
                        {
                            row[newColumn] = value;
                        }
                        else
                        {
                            //Delete new column, because cannot convert all values
                            //Must convert all - not only some.
                            column.Table.Columns.Remove(newColumn);
                            return false;
                        }
                    }

                    //Delete original column
                    column.Table.Columns.Remove(column);
                    newColumn.ColumnName = newColumn.ColumnName.Replace(temp, string.Empty);
                }
                else
                {
                    //Delete new column, because cannot convert
                    column.Table.Columns.Remove(newColumn);
                    return false;
                }
            }
        }
        else
        {
            //Delete original column (now rows to convert...)
            column.Table.Columns.Remove(column);
            newColumn.ColumnName = newColumn.ColumnName.Replace(temp, string.Empty);
        }

        return true;
    }

    public static void ChangeDataType<TFrom, TTo>(this DataColumn column, Func<TFrom, TTo> convert)
    {
        string temp = "_1_TEMP_1_";

        var newColumn = new DataColumn(column.ColumnName + temp, typeof(TTo));
        column.Table.Columns.Add(newColumn);

        foreach (DataRow row in column.Table.Rows)
        {
            var value = row.Field<TFrom>(column);
            row[newColumn] = convert(value);
        }

        //Delete original column
        int ordinal = column.Ordinal;
        column.Table.Columns.Remove(column);
        newColumn.ColumnName = newColumn.ColumnName.Replace(temp, string.Empty);
        newColumn.SetOrdinal(ordinal);
    }

    public static int ColumnLength(this DataColumn column)
    {
        int length = 0;
        int i = 0;
        foreach (DataRow row in column.Table.Rows)
        {
            i = row[column.ColumnName].ToString().Length;
            if (i > length)
            {
                length = i;
            }
        }
        return length;
    }
}
using System;
using System.Data;
using Extenso.Reflection;

namespace Extenso.Data
{
    public static class DataColumnExtensions
    {
        public static bool ChangeDataType<T>(this DataColumn column)
        {
            string temp = "_1_TEMP_1_";

            DataColumn newColumn = new DataColumn(column.ColumnName + temp, typeof(T));
            column.Table.Columns.Add(newColumn);

            if (column.Table.Rows.Count > 0)
            {
                if (typeof(T) == typeof(string))
                {
                    bool isEnum = column.DataType.IsEnum;

                    foreach (DataRow row in column.Table.Rows)
                    {
                        if (isEnum)
                        {
                            row[newColumn] = Enum.ToObject(column.DataType, Convert.ToInt32(row[column])).ToString();
                        }
                        else { row[newColumn] = row[column].ToString(); }
                    }

                    //Delete original column
                    column.Table.Columns.Remove(column);
                    newColumn.ColumnName = newColumn.ColumnName.Replace(temp, string.Empty);
                }
                else
                {
                    T test = default(T);
                    bool success = column.Table.Rows[0][column].ToString().TryParseOrDefault(out test);

                    if (success)
                    {
                        foreach (DataRow row in column.Table.Rows)
                        {
                            T value = default(T);
                            if (row[column].ToString().TryParseOrDefault(out value))
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
}
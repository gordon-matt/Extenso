namespace Extenso.Windows.Forms;

public static class DataGridViewExtensions
{
    extension(DataGridView dataGridView)
    {
        /// <summary>
        /// <para>Sets every column's AutoSizeMode to DataGridViewAutoSizeColumnMode.AllCells</para>
        /// <para>If ColumnLength of column is less than 'columnLengthLimit' argument.</para>
        /// <para>Otherwise, sets width To 'maxColumnWidth' argument</para>
        /// </summary>
        /// <param name="columnLengthLimit">Number of characters allowed for AutoSizeMode to be set to AllCells</param>
        /// <param name="maxColumnWidth">Width to set for columns exceeding 'columnLengthLimit' argument</param>
        public void AutoSizeColumnsWithTrim(int columnLengthLimit, int maxColumnWidth)
        {
            int columnLength;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                columnLength = column.ColumnLength();

                if (columnLength > columnLengthLimit)
                {
                    column.Width = maxColumnWidth;
                }
                else
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
        }
    }

    extension(DataGridViewColumn column)
    {
        /// <summary>
        /// <para>Iterates over all rows in the DataGridView and finds the largest</para>
        /// <para>character length for this column</para>
        /// </summary>
        /// <returns>System.Int32 representing the max character length for this column</returns>
        public int ColumnLength()
        {
            int length = 0;
            int i;
            foreach (DataGridViewRow row in column.DataGridView.Rows)
            {
                i = row.Cells[column.Name].Value.ToString().Length;
                if (i > length)
                {
                    length = i;
                }
            }
            return length;
        }
    }
}
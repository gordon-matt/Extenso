namespace Extenso.Windows.Forms.Controls;

/// <summary>
/// Represents the data grid view bar graph column.
/// </summary>
public class DataGridViewBarGraphColumn : DataGridViewColumn
{
    /// <summary>
    /// The max value.
    /// </summary>
    public long MaxValue;

    private bool needsRecalc = true;

    /// <summary>
    /// Initializes a new instance of the DataGridViewBarGraphColumn class.
    /// </summary>
    public DataGridViewBarGraphColumn()
    {
        this.CellTemplate = new DataGridViewBarGraphCell();
        this.ReadOnly = true;
    }

    /// <summary>
    /// Performs the calc max value operation.
    /// </summary>
    public void CalcMaxValue()
    {
        if (needsRecalc)
        {
            int colIndex = this.DisplayIndex;
            for (int rowIndex = 0; rowIndex < this.DataGridView.Rows.Count; rowIndex++)
            {
                var row = this.DataGridView.Rows[rowIndex];

                try
                { MaxValue = Math.Max(MaxValue, Convert.ToInt64(row.Cells[colIndex].Value)); }
                catch
                { MaxValue = 1; }
            }
            needsRecalc = false;
        }
    }
}
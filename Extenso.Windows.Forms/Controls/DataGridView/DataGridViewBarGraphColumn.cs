namespace Extenso.Windows.Forms.Controls;

public class DataGridViewBarGraphColumn : DataGridViewColumn
{
    public long MaxValue;

    private bool needsRecalc = true;

    public DataGridViewBarGraphColumn()
    {
        this.CellTemplate = new DataGridViewBarGraphCell();
        this.ReadOnly = true;
    }

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
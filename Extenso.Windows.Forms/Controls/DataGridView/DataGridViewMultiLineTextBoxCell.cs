namespace Extenso.Windows.Forms.Controls;

public class DataGridViewMultiLineTextBoxCell : DataGridViewTextBoxCell
{
    public DataGridViewMultiLineTextBoxCell()
        : base()
    {
    }

    public override object DefaultNewRowValue =>
            // Use the current date and time as the default value.
            null;

    public override Type EditType =>
            // Return the type of the editing contol that DataGridViewMultiLineTextBoxCell uses.
            typeof(DataGridViewMultiLineTextBoxEditingControl);

    public override Type ValueType =>
            // Return the type of the value that DataGridViewMultiLineTextBoxCell contains.
            typeof(string);

    public override void InitializeEditingControl(int rowIndex, object
                    initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
    {
        // Set the value of the editing control to the current cell value.
        base.InitializeEditingControl(rowIndex, initialFormattedValue,
            dataGridViewCellStyle);
        var ctl =
            DataGridView.EditingControl as DataGridViewMultiLineTextBoxEditingControl;

        DataGridViewMultiLineTextBoxOptions.multiline =
            (this.DataGridView.Columns[this.ColumnIndex] as DataGridViewMultiLineTextBoxColumn).MultiLine;
        DataGridViewMultiLineTextBoxOptions.wordwrap =
            (this.DataGridView.Columns[this.ColumnIndex] as DataGridViewMultiLineTextBoxColumn).WordWrap;

        ctl.Multiline = DataGridViewMultiLineTextBoxOptions.multiline;
        ctl.WordWrap = DataGridViewMultiLineTextBoxOptions.wordwrap;

        try
        {
            ctl.Text = this.Value.ToString();
        }
        catch (Exception)
        {
        }
    }
}
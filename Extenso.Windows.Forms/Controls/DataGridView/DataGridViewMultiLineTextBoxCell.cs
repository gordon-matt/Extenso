namespace Extenso.Windows.Forms.Controls;

public class DataGridViewMultiLineTextBoxCell : DataGridViewTextBoxCell
{
    public DataGridViewMultiLineTextBoxCell()
        : base()
    {
    }

    public override object DefaultNewRowValue
    {
        get
        {
            // Use the current date and time as the default value.
            return null;
        }
    }

    public override Type EditType
    {
        get
        {
            // Return the type of the editing contol that DataGridViewMultiLineTextBoxCell uses.
            return typeof(DataGridViewMultiLineTextBoxEditingControl);
        }
    }

    public override Type ValueType
    {
        get
        {
            // Return the type of the value that DataGridViewMultiLineTextBoxCell contains.
            return typeof(string);
        }
    }

    public override void InitializeEditingControl(int rowIndex, object
                    initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
    {
        // Set the value of the editing control to the current cell value.
        base.InitializeEditingControl(rowIndex, initialFormattedValue,
            dataGridViewCellStyle);
        DataGridViewMultiLineTextBoxEditingControl ctl =
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
namespace Extenso.Windows.Forms.Controls;

public class DataGridViewNumericTextBoxCell : DataGridViewTextBoxCell
{
    public DataGridViewNumericTextBoxCell()
        : base()
    {
    }

    public override object DefaultNewRowValue =>
            // Use the current date and time as the default value.
            null;

    public override Type EditType =>
            // Return the type of the editing contol that  DataGridViewNumericTextBoxCell uses.
            typeof(DataGridViewNumericTextBoxEditingControl);

    public override Type ValueType =>
            // Return the type of the value that  DataGridViewNumericTextBoxCell contains.
            typeof(System.String);

    public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
    {
        // Set the value of the editing control to the current cell value.
        base.InitializeEditingControl(rowIndex, initialFormattedValue,
            dataGridViewCellStyle);
        var ctl =
           DataGridView.EditingControl as DataGridViewNumericTextBoxEditingControl;

        try
        {
            ctl.Text = this.Value.ToString();
        }
        catch (Exception)
        {
        }
    }
}
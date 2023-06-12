namespace Extenso.Windows.Forms.Controls;

public class DataGridViewNumericTextBoxCell : DataGridViewTextBoxCell
{
    public DataGridViewNumericTextBoxCell()
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
            // Return the type of the editing contol that  DataGridViewNumericTextBoxCell uses.
            return typeof(DataGridViewNumericTextBoxEditingControl);
        }
    }

    public override Type ValueType
    {
        get
        {
            // Return the type of the value that  DataGridViewNumericTextBoxCell contains.
            return typeof(System.String);
        }
    }

    public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
    {
        // Set the value of the editing control to the current cell value.
        base.InitializeEditingControl(rowIndex, initialFormattedValue,
            dataGridViewCellStyle);
        DataGridViewNumericTextBoxEditingControl ctl =
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
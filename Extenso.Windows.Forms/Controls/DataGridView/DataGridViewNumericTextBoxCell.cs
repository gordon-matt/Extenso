namespace Extenso.Windows.Forms.Controls;

/// <summary>
/// Represents the data grid view numeric text box cell.
/// </summary>
public class DataGridViewNumericTextBoxCell : DataGridViewTextBoxCell
{
    /// <summary>
    /// Initializes a new instance of the DataGridViewNumericTextBoxCell class.
    /// </summary>
    public DataGridViewNumericTextBoxCell()
        : base()
    {
    }

    /// <inheritdoc/>
    public override object DefaultNewRowValue =>
            // Use the current date and time as the default value.
            null;

    /// <inheritdoc/>
    public override Type EditType =>
            // Return the type of the editing contol that  DataGridViewNumericTextBoxCell uses.
            typeof(DataGridViewNumericTextBoxEditingControl);

    /// <inheritdoc/>
    public override Type ValueType =>
            // Return the type of the value that  DataGridViewNumericTextBoxCell contains.
            typeof(string);

    /// <inheritdoc/>
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
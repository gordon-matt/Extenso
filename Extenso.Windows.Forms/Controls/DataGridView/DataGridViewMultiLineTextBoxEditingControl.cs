using System.ComponentModel;

namespace Extenso.Windows.Forms.Controls;

[ToolboxItem(false)]
internal class DataGridViewMultiLineTextBoxEditingControl : TextBox, IDataGridViewEditingControl
{
    public DataGridViewMultiLineTextBoxEditingControl()
    {
        this.Multiline = DataGridViewMultiLineTextBoxOptions.multiline;
        this.WordWrap = DataGridViewMultiLineTextBoxOptions.wordwrap;
        this.ScrollBars = ScrollBars.Vertical;
    }

    // Implements the IDataGridViewEditingControl
    // .EditingControlDataGridView property.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public DataGridView EditingControlDataGridView { get; set; }

    // Implements the IDataGridViewEditingControl.EditingControlFormattedValue
    // property.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public object EditingControlFormattedValue
    {
        get => this.Text;
        set
        {
            if (value is string)
            {
                this.Text = value.ToString();
            }
        }
    }

    // Implements the IDataGridViewEditingControl.EditingControlRowIndex
    // property.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int EditingControlRowIndex { get; set; }

    // Implements the IDataGridViewEditingControl
    // .EditingControlValueChanged property.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool EditingControlValueChanged { get; set; } = false;

    // Implements the IDataGridViewEditingControl
    // .EditingPanelCursor property.
    public Cursor EditingPanelCursor => base.Cursor;

    // Implements the IDataGridViewEditingControl
    // .RepositionEditingControlOnValueChange property.
    public bool RepositionEditingControlOnValueChange => false;

    // Implements the
    // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
    public void ApplyCellStyleToEditingControl(
        DataGridViewCellStyle dataGridViewCellStyle) => this.Font = dataGridViewCellStyle.Font;

    // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey
    // method.
    public bool EditingControlWantsInputKey(
        Keys key, bool dataGridViewWantsInputKey) => (key & Keys.KeyCode) switch
        {
            Keys.Left or Keys.Up or Keys.Down or Keys.Right or Keys.Home or Keys.End or Keys.PageDown or Keys.PageUp or Keys.Delete or Keys.Back => true,
            _ => false,
        };

    // Implements the
    // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
    public object GetEditingControlFormattedValue(
        DataGridViewDataErrorContexts context) => EditingControlFormattedValue;

    // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit
    // method.
    public void PrepareEditingControlForEdit(bool selectAll)
    {
        // No preparation needs to be done.
    }

    protected override void OnTextChanged(EventArgs e)
    {
        // Notify the DataGridView that the contents of the cell
        // have changed.
        EditingControlValueChanged = true;
        this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
        base.OnTextChanged(e);
    }
}
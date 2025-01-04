using System.ComponentModel;

namespace Extenso.Windows.Forms.Controls;

[ToolboxItem(false)]
internal class DataGridViewNumericTextBoxEditingControl : NumericTextBox, IDataGridViewEditingControl
{
    private DataGridView dataGridView;
    private int rowIndex;
    private bool valueChanged = false;

    public DataGridViewNumericTextBoxEditingControl()
    {
    }

    // Implements the IDataGridViewEditingControl
    // .EditingControlDataGridView property.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public DataGridView EditingControlDataGridView
    {
        get
        {
            return dataGridView;
        }
        set
        {
            dataGridView = value;
        }
    }

    // Implements the IDataGridViewEditingControl.EditingControlFormattedValue
    // property.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public object EditingControlFormattedValue
    {
        get
        {
            return this.Text;
        }
        set
        {
            if (value is String)
            {
                this.Text = value.ToString();
            }
        }
    }

    // Implements the IDataGridViewEditingControl.EditingControlRowIndex
    // property.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int EditingControlRowIndex
    {
        get
        {
            return rowIndex;
        }
        set
        {
            rowIndex = value;
        }
    }

    // Implements the IDataGridViewEditingControl
    // .EditingControlValueChanged property.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool EditingControlValueChanged
    {
        get
        {
            return valueChanged;
        }
        set
        {
            valueChanged = value;
        }
    }

    // Implements the IDataGridViewEditingControl
    // .EditingPanelCursor property.
    public Cursor EditingPanelCursor
    {
        get
        {
            return base.Cursor;
        }
    }

    // Implements the IDataGridViewEditingControl
    // .RepositionEditingControlOnValueChange property.
    public bool RepositionEditingControlOnValueChange
    {
        get
        {
            return false;
        }
    }

    // Implements the
    // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
    public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
    {
        this.Font = dataGridViewCellStyle.Font;
    }

    // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey
    // method.
    public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
    {
        switch (key & Keys.KeyCode)
        {
            case Keys.Left:
            case Keys.Up:
            case Keys.Down:
            case Keys.Right:
            case Keys.Home:
            case Keys.End:
            case Keys.PageDown:
            case Keys.PageUp:
            case Keys.Delete:
            case Keys.Back:
                return true;

            default:
                return false;
        }
    }

    // Implements the
    // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
    public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
    {
        return EditingControlFormattedValue;
    }

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
        valueChanged = true;
        this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
        base.OnTextChanged(e);
    }
}
using System.ComponentModel;

namespace Extenso.Windows.Forms.Controls;

[ToolboxItem(false)]
internal class DataGridViewCalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
{
    #region Properties

    // Implements the IDataGridViewEditingControl
    // .EditingControlDataGridView property.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public DataGridView EditingControlDataGridView { get; set; }

    // Implements the IDataGridViewEditingControl.EditingControlRowIndex
    // property.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public int EditingControlRowIndex { get; set; }

    // Implements the IDataGridViewEditingControl
    // .EditingControlValueChanged property.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool EditingControlValueChanged { get; set; } = false;

    #endregion Properties

    public DataGridViewCalendarEditingControl()
    {
        Format = DataGridViewCalendarColumnDateFormat.DateTimePickerFormat;
        CustomFormat = DataGridViewCalendarColumnDateFormat.CustomFormat;
    }

    // Implements the IDataGridViewEditingControl.EditingControlFormattedValue
    // property.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public object EditingControlFormattedValue
    {
        get => Value.ToString("O");
        set
        {
            if (value is string val)
            {
                Value = DateTime.Parse(val);
            }
        }
    }

    // Implements the IDataGridViewEditingControl
    // .EditingPanelCursor property.
    public Cursor EditingPanelCursor => base.Cursor;

    // Implements the IDataGridViewEditingControl
    // .RepositionEditingControlOnValueChange property.
    public bool RepositionEditingControlOnValueChange => false;

    // Implements the
    // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
    public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
    {
        Font = dataGridViewCellStyle.Font;
        CalendarForeColor = dataGridViewCellStyle.ForeColor;
        CalendarMonthBackground = dataGridViewCellStyle.BackColor;
    }

    // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey method.
    public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
    {
        // Let the DateTimePicker handle the keys listed.
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
                return true;

            case Keys.Delete:
            case Keys.Back:
                var CurCell = EditingControlDataGridView.CurrentCell;
                if (CurCell != null)
                { CurCell.Value = DBNull.Value; }
                if (DataGridViewCalendarColumnDateFormat.DateTimePickerFormat != DateTimePickerFormat.Custom)
                { Format = DateTimePickerFormat.Custom; }
                CustomFormat = " ";
                return false;

            default: return false;
        }
    }

    // Implements the
    // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
    public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts dataGridViewDataErrorContexts) => EditingControlFormattedValue;

    // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit
    // method.
    public void PrepareEditingControlForEdit(bool selectAll)
    {
        // No preparation needs to be done.
    }

    protected override void OnPaint(PaintEventArgs e) => base.OnPaint(e);

    protected override void OnValueChanged(EventArgs eventargs)
    {
        if (CustomFormat == " ")
        {
            switch (DataGridViewCalendarColumnDateFormat.DateTimePickerFormat)
            {
                case DateTimePickerFormat.Long:
                    Format = DateTimePickerFormat.Long;
                    break;

                case DateTimePickerFormat.Short:
                    Format = DateTimePickerFormat.Short;
                    break;

                case DateTimePickerFormat.Time:
                    Format = DateTimePickerFormat.Time;
                    break;

                case DateTimePickerFormat.Custom:
                    Format = DateTimePickerFormat.Custom;
                    CustomFormat = DataGridViewCalendarColumnDateFormat.CustomFormat;
                    break;

                default:
                    break;
            }
        }

        // Notify the DataGridView that the contents of the cell
        // have changed.
        EditingControlValueChanged = true;
        EditingControlDataGridView.NotifyCurrentCellDirty(true);
        base.OnValueChanged(eventargs);
    }
}
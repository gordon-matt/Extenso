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
        this.Format = DataGridViewCalendarColumnDateFormat.DateTimePickerFormat;
        this.CustomFormat = DataGridViewCalendarColumnDateFormat.CustomFormat;
    }

    // Implements the IDataGridViewEditingControl.EditingControlFormattedValue
    // property.
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public object EditingControlFormattedValue
    {
        get
        { return this.Value.ToString("O"); }
        set
        {
            if (value is string)
            { this.Value = DateTime.Parse((string)value); }
        }
    }

    // Implements the IDataGridViewEditingControl
    // .EditingPanelCursor property.
    public Cursor EditingPanelCursor
    {
        get { return base.Cursor; }
    }

    // Implements the IDataGridViewEditingControl
    // .RepositionEditingControlOnValueChange property.
    public bool RepositionEditingControlOnValueChange
    {
        get { return false; }
    }

    // Implements the
    // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
    public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
    {
        this.Font = dataGridViewCellStyle.Font;
        this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
        this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
    }

    // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey method.
    public Boolean EditingControlWantsInputKey(Keys key, Boolean dataGridViewWantsInputKey)
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
                DataGridViewCell CurCell = this.EditingControlDataGridView.CurrentCell;
                if ((CurCell != null))
                { CurCell.Value = DBNull.Value; }
                if (DataGridViewCalendarColumnDateFormat.DateTimePickerFormat != DateTimePickerFormat.Custom)
                { this.Format = DateTimePickerFormat.Custom; }
                this.CustomFormat = " ";
                return false;

            default: return false;
        }
    }

    // Implements the
    // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
    public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts dataGridViewDataErrorContexts)
    { return EditingControlFormattedValue; }

    // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit
    // method.
    public void PrepareEditingControlForEdit(Boolean selectAll)
    {
        // No preparation needs to be done.
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
    }

    protected override void OnValueChanged(EventArgs eventargs)
    {
        if (this.CustomFormat == " ")
        {
            switch (DataGridViewCalendarColumnDateFormat.DateTimePickerFormat)
            {
                case DateTimePickerFormat.Long:
                    this.Format = DateTimePickerFormat.Long;
                    break;

                case DateTimePickerFormat.Short:
                    this.Format = DateTimePickerFormat.Short;
                    break;

                case DateTimePickerFormat.Time:
                    this.Format = DateTimePickerFormat.Time;
                    break;

                case DateTimePickerFormat.Custom:
                    this.Format = DateTimePickerFormat.Custom;
                    this.CustomFormat = DataGridViewCalendarColumnDateFormat.CustomFormat;
                    break;

                default:
                    break;
            }
        }

        // Notify the DataGridView that the contents of the cell
        // have changed.
        EditingControlValueChanged = true;
        this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
        base.OnValueChanged(eventargs);
    }
}
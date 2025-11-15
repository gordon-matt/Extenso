using System.ComponentModel;

namespace Extenso.Windows.Forms.Controls;

public class DataGridViewCalendarColumn : DataGridViewColumn
{
    public DataGridViewCalendarColumn() : base(new DataGridViewCalendarCell())
    {
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public override DataGridViewCell CellTemplate
    {
        get => base.CellTemplate;
        set
        {
            // Ensure that the cell used for the template is a CalendarCell.
            if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewCalendarCell)))
            { throw new InvalidCastException("Must be a CalendarCell"); }
            base.CellTemplate = value;
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string CustomFormat
    {
        get => field;
        set
        {
            field = value;
            DataGridViewCalendarColumnDateFormat.CustomFormat = field;
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public DateTimePickerFormat DateTimePickerFormat
    {
        get => field;
        set
        {
            field = value;
            DataGridViewCalendarColumnDateFormat.DateTimePickerFormat = field;
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ShowCheckBox { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ShowUpDown { get; set; }

    public override object Clone()
    {
        var dataGridViewCalendarColumn = (DataGridViewCalendarColumn)base.Clone();
        dataGridViewCalendarColumn.ShowCheckBox = ShowCheckBox;
        dataGridViewCalendarColumn.ShowUpDown = ShowUpDown;
        dataGridViewCalendarColumn.DateTimePickerFormat = DateTimePickerFormat;
        dataGridViewCalendarColumn.CustomFormat = CustomFormat;
        return dataGridViewCalendarColumn;
    }
}
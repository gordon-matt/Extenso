using System.ComponentModel;

namespace Extenso.Windows.Forms.Controls;

public class DataGridViewCalendarColumn : DataGridViewColumn
{
    private string customFormat;

    private DateTimePickerFormat dateTimePickerFormat;

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
        get => customFormat;
        set
        {
            customFormat = value;
            DataGridViewCalendarColumnDateFormat.CustomFormat = customFormat;
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public DateTimePickerFormat DateTimePickerFormat
    {
        get => dateTimePickerFormat;
        set
        {
            dateTimePickerFormat = value;
            DataGridViewCalendarColumnDateFormat.DateTimePickerFormat = dateTimePickerFormat;
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ShowCheckBox { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ShowUpDown { get; set; }

    public override object Clone()
    {
        var dataGridViewCalendarColumn = (DataGridViewCalendarColumn)base.Clone();
        dataGridViewCalendarColumn.ShowCheckBox = this.ShowCheckBox;
        dataGridViewCalendarColumn.ShowUpDown = this.ShowUpDown;
        dataGridViewCalendarColumn.dateTimePickerFormat = this.dateTimePickerFormat;
        dataGridViewCalendarColumn.customFormat = this.customFormat;
        return dataGridViewCalendarColumn;
    }
}
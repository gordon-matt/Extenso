namespace Extenso.Windows.Forms.Controls;

public class DataGridViewCalendarColumn : DataGridViewColumn
{
    private string customFormat;

    private DateTimePickerFormat dateTimePickerFormat;

    public DataGridViewCalendarColumn() : base(new DataGridViewCalendarCell())
    {
    }

    public override DataGridViewCell CellTemplate
    {
        get
        { return base.CellTemplate; }
        set
        {
            // Ensure that the cell used for the template is a CalendarCell.
            if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewCalendarCell)))
            { throw new InvalidCastException("Must be a CalendarCell"); }
            base.CellTemplate = value;
        }
    }

    public string CustomFormat
    {
        get { return customFormat; }
        set
        {
            customFormat = value;
            DataGridViewCalendarColumnDateFormat.CustomFormat = customFormat;
        }
    }

    public DateTimePickerFormat DateTimePickerFormat
    {
        get { return dateTimePickerFormat; }
        set
        {
            dateTimePickerFormat = value;
            DataGridViewCalendarColumnDateFormat.DateTimePickerFormat = dateTimePickerFormat;
        }
    }

    public bool ShowCheckBox { get; set; }

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
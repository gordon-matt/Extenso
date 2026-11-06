using System.ComponentModel;

namespace Extenso.Windows.Forms.Controls;

/// <summary>
/// Represents the data grid view calendar column.
/// </summary>
public class DataGridViewCalendarColumn : DataGridViewColumn
{
    /// <summary>
    /// Initializes a new instance of the DataGridViewCalendarColumn class.
    /// </summary>
    public DataGridViewCalendarColumn() : base(new DataGridViewCalendarCell())
    {
    }

    /// <inheritdoc/>
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

    /// <summary>
    /// Gets or sets the custom format.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the date time picker format.
    /// </summary>
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

    /// <summary>
    /// Gets or sets a value indicating whether this instance is show check box.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ShowCheckBox { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is show up down.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ShowUpDown { get; set; }

    /// <inheritdoc/>
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
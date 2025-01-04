namespace Extenso.Windows.Forms.Controls;

public class DataGridViewCalendarCell : DataGridViewTextBoxCell
{
    public DataGridViewCalendarCell() : base()
    {
        switch (DataGridViewCalendarColumnDateFormat.DateTimePickerFormat)
        {
            case DateTimePickerFormat.Long:
                //this.Style.Format = "MMMM dd, yyyy";
                this.Style.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.LongDatePattern;
                break;

            case DateTimePickerFormat.Short:
                //this.Style.Format = "d/M/yyyy";
                this.Style.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                break;

            case DateTimePickerFormat.Time:
                //this.Style.Format = "hh:mm";
                this.Style.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortTimePattern;
                break;

            case DateTimePickerFormat.Custom:
                this.Style.Format = DataGridViewCalendarColumnDateFormat.CustomFormat;
                break;

            default:
                break;
        }
    }

    public override object DefaultNewRowValue
    {
        // Use the current date and time as the default value.
        get { return null; } //return DateTime.Now; }
    }

    public override Type EditType
    {
        // Return the type of the editing contol that CalendarCell uses.
        get { return typeof(DataGridViewCalendarEditingControl); }
    }

    public override Type ValueType
    {
        // Return the type of the value that CalendarCell contains.
        get { return typeof(DateTime); }
    }

    public override void InitializeEditingControl(Int32 rowIndex, Object initialFormattedValue,
                                DataGridViewCellStyle dataGridViewCellStyle)
    {
        // Set the value of the editing control to the current cell value.
        base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
        var dataGridViewCalendarEditingControl = DataGridView.EditingControl as DataGridViewCalendarEditingControl;

        dataGridViewCalendarEditingControl.ShowCheckBox = (DataGridView.Columns[ColumnIndex] as DataGridViewCalendarColumn).ShowCheckBox;
        dataGridViewCalendarEditingControl.ShowUpDown = (DataGridView.Columns[ColumnIndex] as DataGridViewCalendarColumn).ShowUpDown;

        try
        {
            dataGridViewCalendarEditingControl.Value = (DateTime)this.Value;

            switch (DataGridViewCalendarColumnDateFormat.DateTimePickerFormat)
            {
                case DateTimePickerFormat.Long:
                    dataGridViewCalendarEditingControl.Format = DateTimePickerFormat.Long;
                    break;

                case DateTimePickerFormat.Short:
                    dataGridViewCalendarEditingControl.Format = DateTimePickerFormat.Short;
                    break;

                case DateTimePickerFormat.Time:
                    dataGridViewCalendarEditingControl.Format = DateTimePickerFormat.Time;
                    break;

                case DateTimePickerFormat.Custom:
                    dataGridViewCalendarEditingControl.Format = DateTimePickerFormat.Custom;
                    dataGridViewCalendarEditingControl.CustomFormat = DataGridViewCalendarColumnDateFormat.CustomFormat;
                    break;

                default:
                    break;
            }
        }
        catch   //ArgumentOutOfRangeException
        //InvalidCastException
        {
        }
    }
}
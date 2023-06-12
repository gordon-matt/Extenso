namespace Extenso.Windows.Forms.Controls;

public class DataGridViewNumericTextBoxColumn : DataGridViewColumn
{
    public DataGridViewNumericTextBoxColumn()
        : base(new DataGridViewNumericTextBoxCell())
    {
    }

    public override DataGridViewCell CellTemplate
    {
        get
        {
            return base.CellTemplate;
        }
        set
        {
            // Ensure that the cell used for the template is a NumericTextBoxCell.
            if (value != null &&
                !value.GetType().IsAssignableFrom(typeof(DataGridViewNumericTextBoxCell)))
            {
                throw new InvalidCastException("Must be a NumericTextBoxCell");
            }
            base.CellTemplate = value;
        }
    }

    public override object Clone()
    {
        DataGridViewNumericTextBoxColumn col = (DataGridViewNumericTextBoxColumn)base.Clone();
        return col;
    }
}
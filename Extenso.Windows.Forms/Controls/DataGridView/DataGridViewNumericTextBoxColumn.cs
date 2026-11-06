using System.ComponentModel;

namespace Extenso.Windows.Forms.Controls;

/// <summary>
/// Represents the data grid view numeric text box column.
/// </summary>
public class DataGridViewNumericTextBoxColumn : DataGridViewColumn
{
    /// <summary>
    /// Initializes a new instance of the DataGridViewNumericTextBoxColumn class.
    /// </summary>
    public DataGridViewNumericTextBoxColumn()
        : base(new DataGridViewNumericTextBoxCell())
    {
    }

    /// <inheritdoc/>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public override DataGridViewCell CellTemplate
    {
        get => base.CellTemplate;
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

    /// <inheritdoc/>
    public override object Clone()
    {
        var col = (DataGridViewNumericTextBoxColumn)base.Clone();
        return col;
    }
}
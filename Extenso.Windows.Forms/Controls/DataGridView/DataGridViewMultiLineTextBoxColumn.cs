using System.ComponentModel;

namespace Extenso.Windows.Forms.Controls;

public class DataGridViewMultiLineTextBoxColumn : DataGridViewColumn
{
    public DataGridViewMultiLineTextBoxColumn()
        : base(new DataGridViewMultiLineTextBoxCell())
    {
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public override DataGridViewCell CellTemplate
    {
        get => base.CellTemplate;
        set
        {
            // Ensure that the cell used for the template is a MultiLineTextBoxCell.
            if (value != null &&
                !value.GetType().IsAssignableFrom(typeof(DataGridViewMultiLineTextBoxCell)))
            {
                throw new InvalidCastException("Must be a MultiLineTextBoxCell");
            }
            base.CellTemplate = value;
        }
    }

    #region MultiLine Options

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool MultiLine { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool WordWrap { get; set; }

    public override object Clone()
    {
        var col =
            (DataGridViewMultiLineTextBoxColumn)base.Clone();
        col.MultiLine = this.MultiLine;
        col.WordWrap = this.WordWrap;
        return col;
    }

    #endregion MultiLine Options
}
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
        get
        {
            return base.CellTemplate;
        }
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

    private bool multiLine;

    private bool wordWrap;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool MultiLine
    {
        get
        {
            return multiLine;
        }
        set
        {
            multiLine = value;
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool WordWrap
    {
        get
        {
            return wordWrap;
        }
        set
        {
            wordWrap = value;
        }
    }

    public override object Clone()
    {
        DataGridViewMultiLineTextBoxColumn col =
            (DataGridViewMultiLineTextBoxColumn)base.Clone();
        col.multiLine = this.multiLine;
        col.wordWrap = this.wordWrap;
        return col;
    }

    #endregion MultiLine Options
}
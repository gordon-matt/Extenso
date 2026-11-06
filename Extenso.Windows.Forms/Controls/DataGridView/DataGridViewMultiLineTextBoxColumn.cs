using System.ComponentModel;

namespace Extenso.Windows.Forms.Controls;

/// <summary>
/// Represents the data grid view multi line text box column.
/// </summary>
public class DataGridViewMultiLineTextBoxColumn : DataGridViewColumn
{
    /// <summary>
    /// Initializes a new instance of the DataGridViewMultiLineTextBoxColumn class.
    /// </summary>
    public DataGridViewMultiLineTextBoxColumn()
        : base(new DataGridViewMultiLineTextBoxCell())
    {
    }

    /// <inheritdoc/>
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

    /// <summary>
    /// Gets or sets a value indicating whether this instance is multi line.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool MultiLine { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is word wrap.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool WordWrap { get; set; }

    /// <inheritdoc/>
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
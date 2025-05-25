//Source: http://msdn2.microsoft.com/en-us/library/aa730881(VS.80).aspx
using System.ComponentModel;
using System.Diagnostics;

namespace Extenso.Windows.Forms.Controls;

/// <summary>
/// Custom column type dedicated to the DataGridViewNumericUpDownCell cell type.
/// </summary>
[DebuggerDisplay("Name = {Name}, Index = {Index}")]
public class DataGridViewNumericUpDownColumn : DataGridViewColumn
{
    /// <summary>
    /// Constructor for the DataGridViewNumericUpDownColumn class.
    /// </summary>
    public DataGridViewNumericUpDownColumn()
        : base(new DataGridViewNumericUpDownCell())
    {
    }

    /// <summary>
    /// Represents the implicit cell that gets cloned when adding rows to the grid.
    /// </summary>
    [
        Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
    ]
    public override DataGridViewCell CellTemplate
    {
        get => base.CellTemplate;
        set
        {
            var dataGridViewNumericUpDownCell = value as DataGridViewNumericUpDownCell;
            if (value != null && dataGridViewNumericUpDownCell == null)
            {
                throw new InvalidCastException("Value provided for CellTemplate must be of type DataGridViewNumericUpDownElements.DataGridViewNumericUpDownCell or derive from it.");
            }
            base.CellTemplate = value;
        }
    }

    /// <summary>
    /// Replicates the DecimalPlaces property of the DataGridViewNumericUpDownCell cell type.
    /// </summary>
    [
        Category("Appearance"),
        DefaultValue(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultDecimalPlaces),
        Description("Indicates the number of decimal places to display.")
    ]
    public int DecimalPlaces
    {
        get => this.NumericUpDownCellTemplate == null
                ? throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.")
                : NumericUpDownCellTemplate.DecimalPlaces;
        set
        {
            if (this.NumericUpDownCellTemplate == null)
            {
                throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
            }
            // Update the template cell so that subsequent cloned cells use the new value.
            this.NumericUpDownCellTemplate.DecimalPlaces = value;
            if (this.DataGridView != null)
            {
                // Update all the existing DataGridViewNumericUpDownCell cells in the column accordingly.
                var dataGridViewRows = this.DataGridView.Rows;
                int rowCount = dataGridViewRows.Count;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    // Be careful not to unshare rows unnecessarily.
                    // This could have severe performance repercussions.
                    var dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                    var dataGridViewCell = dataGridViewRow.Cells[this.Index] as DataGridViewNumericUpDownCell;
                    // Call the internal SetDecimalPlaces method instead of the property to avoid invalidation
                    // of each cell. The whole column is invalidated later in a single operation for better performance.
                    dataGridViewCell?.SetDecimalPlaces(rowIndex, value);
                }
                this.DataGridView.InvalidateColumn(this.Index);
                // TODO: Call the grid's autosizing methods to autosize the column, rows, column headers / row headers as needed.
            }
        }
    }

    /// <summary>
    /// Replicates the Increment property of the DataGridViewNumericUpDownCell cell type.
    /// </summary>
    [
        Category("Data"),
        Description("Indicates the amount to increment or decrement on each button click.")
    ]
    public decimal Increment
    {
        get => this.NumericUpDownCellTemplate == null
                ? throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.")
                : NumericUpDownCellTemplate.Increment;
        set
        {
            if (this.NumericUpDownCellTemplate == null)
            {
                throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
            }
            this.NumericUpDownCellTemplate.Increment = value;
            if (this.DataGridView != null)
            {
                var dataGridViewRows = this.DataGridView.Rows;
                int rowCount = dataGridViewRows.Count;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                    var dataGridViewCell = dataGridViewRow.Cells[this.Index] as DataGridViewNumericUpDownCell;
                    dataGridViewCell?.SetIncrement(rowIndex, value);
                }
            }
        }
    }

    /// <summary>
    /// Replicates the Maximum property of the DataGridViewNumericUpDownCell cell type.
    /// </summary>
    [
        Category("Data"),
        Description("Indicates the maximum value for the numeric up-down cells."),
        RefreshProperties(RefreshProperties.All)
    ]
    public decimal Maximum
    {
        get => this.NumericUpDownCellTemplate == null
                ? throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.")
                : NumericUpDownCellTemplate.Maximum;
        set
        {
            if (this.NumericUpDownCellTemplate == null)
            {
                throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
            }
            this.NumericUpDownCellTemplate.Maximum = value;
            if (this.DataGridView != null)
            {
                var dataGridViewRows = this.DataGridView.Rows;
                int rowCount = dataGridViewRows.Count;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                    var dataGridViewCell = dataGridViewRow.Cells[this.Index] as DataGridViewNumericUpDownCell;
                    dataGridViewCell?.SetMaximum(rowIndex, value);
                }
                this.DataGridView.InvalidateColumn(this.Index);
                // TODO: This column and/or grid rows may need to be autosized depending on their
                //       autosize settings. Call the autosizing methods to autosize the column, rows,
                //       column headers / row headers as needed.
            }
        }
    }

    /// <summary>
    /// Replicates the Minimum property of the DataGridViewNumericUpDownCell cell type.
    /// </summary>
    [
        Category("Data"),
        Description("Indicates the minimum value for the numeric up-down cells."),
        RefreshProperties(RefreshProperties.All)
    ]
    public decimal Minimum
    {
        get => this.NumericUpDownCellTemplate == null
            ? throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.")
            : NumericUpDownCellTemplate.Minimum;
        set
        {
            if (this.NumericUpDownCellTemplate == null)
            {
                throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
            }
            this.NumericUpDownCellTemplate.Minimum = value;
            if (this.DataGridView != null)
            {
                var dataGridViewRows = this.DataGridView.Rows;
                int rowCount = dataGridViewRows.Count;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                    var dataGridViewCell = dataGridViewRow.Cells[this.Index] as DataGridViewNumericUpDownCell;
                    dataGridViewCell?.SetMinimum(rowIndex, value);
                }
                this.DataGridView.InvalidateColumn(this.Index);
                // TODO: This column and/or grid rows may need to be autosized depending on their
                //       autosize settings. Call the autosizing methods to autosize the column, rows,
                //       column headers / row headers as needed.
            }
        }
    }

    /// <summary>
    /// Replicates the ThousandsSeparator property of the DataGridViewNumericUpDownCell cell type.
    /// </summary>
    [
        Category("Data"),
        DefaultValue(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultThousandsSeparator),
        Description("Indicates whether the thousands separator will be inserted between every three decimal digits.")
    ]
    public bool ThousandsSeparator
    {
        get => this.NumericUpDownCellTemplate == null
                ? throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.")
                : NumericUpDownCellTemplate.ThousandsSeparator;
        set
        {
            if (this.NumericUpDownCellTemplate == null)
            {
                throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
            }
            this.NumericUpDownCellTemplate.ThousandsSeparator = value;
            if (this.DataGridView != null)
            {
                var dataGridViewRows = this.DataGridView.Rows;
                int rowCount = dataGridViewRows.Count;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                    var dataGridViewCell = dataGridViewRow.Cells[this.Index] as DataGridViewNumericUpDownCell;
                    dataGridViewCell?.SetThousandsSeparator(rowIndex, value);
                }
                this.DataGridView.InvalidateColumn(this.Index);
                // TODO: This column and/or grid rows may need to be autosized depending on their
                //       autosize settings. Call the autosizing methods to autosize the column, rows,
                //       column headers / row headers as needed.
            }
        }
    }

    /// <summary>
    /// Small utility function that returns the template cell as a DataGridViewNumericUpDownCell
    /// </summary>
    private DataGridViewNumericUpDownCell NumericUpDownCellTemplate => (DataGridViewNumericUpDownCell)this.CellTemplate;

    /// Indicates whether the Increment property should be persisted.
    private bool ShouldSerializeIncrement() => !this.Increment.Equals(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultIncrement);

    /// Indicates whether the Maximum property should be persisted.
    private bool ShouldSerializeMaximum() => !this.Maximum.Equals(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMaximum);

    /// Indicates whether the Maximum property should be persisted.
    private bool ShouldSerializeMinimum() => !this.Minimum.Equals(DataGridViewNumericUpDownCell.DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMinimum);
}
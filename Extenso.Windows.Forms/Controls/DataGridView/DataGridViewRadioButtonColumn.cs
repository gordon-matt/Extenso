//Source: http://msdn2.microsoft.com/en-us/library/aa730882(VS.80).aspx
using System.ComponentModel;
using System.Diagnostics;

namespace Extenso.Windows.Forms.Controls;

/// <summary>
/// Custom column type dedicated to the DataGridViewRadioButtonCell cell type.
/// </summary>
[DebuggerDisplay("Name = {Name}, Index = {Index}")]
public class DataGridViewRadioButtonColumn : DataGridViewColumn
{
    /// <summary>
    /// Column constructor that uses a default DataGridViewRadioButtonCell cell for its CellTemplate.
    /// </summary>
    public DataGridViewRadioButtonColumn()
        : base(new DataGridViewRadioButtonCell())
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
            var dataGridViewRadioButtonCell = value as DataGridViewRadioButtonCell;
            if (value != null && dataGridViewRadioButtonCell == null)
            {
                throw new InvalidCastException("Value provided for CellTemplate must be of type DataGridViewRadioButtonElements.DataGridViewRadioButtonCell or derive from it.");
            }
            base.CellTemplate = value;
        }
    }

    /// <summary>
    /// Replicates the DataSource property of the DataGridViewRadioButtonCell cell type.
    /// </summary>
    [
        AttributeProvider(typeof(IListSource)),
        Category("Data"),
        DefaultValue(null),
        Description("The data source that populates the radio buttons."),
        RefreshProperties(RefreshProperties.Repaint)
    ]
    public object DataSource
    {
        get => this.RadioButtonCellTemplate == null
                ? throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.")
                : RadioButtonCellTemplate.DataSource;
        set
        {
            if (this.RadioButtonCellTemplate == null)
            {
                throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
            }
            this.RadioButtonCellTemplate.DataSource = value;
            if (this.DataGridView != null)
            {
                var dataGridViewRows = this.DataGridView.Rows;
                int rowCount = dataGridViewRows.Count;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                    if (dataGridViewRow.Cells[this.Index] is DataGridViewRadioButtonCell dataGridViewCell)
                    {
                        dataGridViewCell.DataSource = value;
                    }
                }
                this.DataGridView.InvalidateColumn(this.Index);
                // TODO: This column and/or grid rows may need to be autosized depending on their
                //       autosize settings. Call the autosizing methods to autosize the column, rows,
                //       column headers / row headers as needed.
            }
        }
    }

    /// <summary>
    /// Replicates the DisplayMember property of the DataGridViewRadioButtonCell cell type.
    /// </summary>
    [
        Category("Data"),
        DefaultValue(""),
        Description("A string that specifies the property or column from which to retrieve strings for display in the radio buttons."),
        Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor)),
        TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design")
    ]
    public string DisplayMember
    {
        get => this.RadioButtonCellTemplate == null
                ? throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.")
                : RadioButtonCellTemplate.DisplayMember;
        set
        {
            if (this.RadioButtonCellTemplate == null)
            {
                throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
            }
            this.RadioButtonCellTemplate.DisplayMember = value;
            if (this.DataGridView != null)
            {
                var dataGridViewRows = this.DataGridView.Rows;
                int rowCount = dataGridViewRows.Count;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                    if (dataGridViewRow.Cells[this.Index] is DataGridViewRadioButtonCell dataGridViewCell)
                    {
                        dataGridViewCell.DisplayMember = value;
                    }
                }
                this.DataGridView.InvalidateColumn(this.Index);
                // TODO: Add code to autosize the column and rows, the column headers,
                // the row headers, depending on the autosize settings of the grid.
            }
        }
    }

    /// <summary>
    /// Replicates the Items property of the DataGridViewRadioButtonCell cell type.
    /// </summary>
    [
        Category("Data"),
        Description("The collection of objects used as entries for the radio buttons."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))
    ]
    public DataGridViewRadioButtonCell.ObjectCollection Items => this.RadioButtonCellTemplate == null
                ? throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.")
                : RadioButtonCellTemplate.Items;

    /// <summary>
    /// Replicates the MaxDisplayedItems property of the DataGridViewRadioButtonCell cell type.
    /// </summary>
    [
        Category("Behavior"),
        DefaultValue(DataGridViewRadioButtonCell.DATAGRIDVIEWRADIOBUTTONCELL_defaultMaxDisplayedItems),
        Description("The maximum number of radio buttons to display in the cells of the column.")
    ]
    public int MaxDisplayedItems
    {
        get => this.RadioButtonCellTemplate == null
                ? throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.")
                : RadioButtonCellTemplate.MaxDisplayedItems;
        set
        {
            if (this.MaxDisplayedItems != value)
            {
                this.RadioButtonCellTemplate.MaxDisplayedItems = value;
                if (this.DataGridView != null)
                {
                    var dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        var dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        if (dataGridViewRow.Cells[this.Index] is DataGridViewRadioButtonCell dataGridViewCell)
                        {
                            dataGridViewCell.MaxDisplayedItemsInternal = value;
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                    // TODO: Add code to autosize the column and rows, the column headers,
                    // the row headers, depending on the autosize settings of the grid.
                }
            }
        }
    }

    /// <summary>
    /// Small utility function that returns the template cell as a DataGridViewRadioButtonCell.
    /// </summary>
    private DataGridViewRadioButtonCell RadioButtonCellTemplate => (DataGridViewRadioButtonCell)this.CellTemplate;

    /// <summary>
    /// Replicates the ValueMember property of the DataGridViewRadioButtonCell cell type.
    /// </summary>
    [
        Category("Data"),
        DefaultValue(""),
        Description("A string that specifies the property or column from which to get values that correspond to the radio buttons."),
        Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor)),
        TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design")
    ]
    public string ValueMember
    {
        get => this.RadioButtonCellTemplate == null
                ? throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.")
                : RadioButtonCellTemplate.ValueMember;
        set
        {
            if (this.RadioButtonCellTemplate == null)
            {
                throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
            }
            this.RadioButtonCellTemplate.ValueMember = value;
            if (this.DataGridView != null)
            {
                var dataGridViewRows = this.DataGridView.Rows;
                int rowCount = dataGridViewRows.Count;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                    if (dataGridViewRow.Cells[this.Index] is DataGridViewRadioButtonCell dataGridViewCell)
                    {
                        dataGridViewCell.ValueMember = value;
                    }
                }
                this.DataGridView.InvalidateColumn(this.Index);
                // TODO: Add code to autosize the column and rows, the column headers,
                // the row headers, depending on the autosize settings of the grid.
            }
        }
    }

    /// <summary>
    /// Call this public method when the Items collection of this column's CellTemplate was changed.
    /// Updates the items collection of each existing DataGridViewRadioButtonCell in the column.
    /// </summary>
    public void NotifyItemsCollectionChanged()
    {
        if (this.DataGridView != null)
        {
            var dataGridViewRows = this.DataGridView.Rows;
            int rowCount = dataGridViewRows.Count;
            var cellTemplate = this.CellTemplate as DataGridViewRadioButtonCell;
            object[] items = new object[cellTemplate.Items.Count];
            cellTemplate.Items.CopyTo(items, 0);
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                var dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                if (dataGridViewRow.Cells[this.Index] is DataGridViewRadioButtonCell dataGridViewCell)
                {
                    dataGridViewCell.Items.Clear();
                    dataGridViewCell.Items.AddRange(items);
                }
            }
            this.DataGridView.InvalidateColumn(this.Index);
            // This column and/or rows may need to be autosized.
        }
    }
}
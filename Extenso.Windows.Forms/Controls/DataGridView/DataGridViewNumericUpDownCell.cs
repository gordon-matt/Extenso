﻿//Source: http://msdn2.microsoft.com/en-us/library/aa730881(VS.80).aspx
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Extenso.Windows.Forms.Controls;

/// <summary>
/// Defines a NumericUpDown cell type for the System.Windows.Forms.DataGridView control
/// </summary>
[DebuggerDisplay("ColumnIndex = {ColumnIndex}, RowIndex = {RowIndex}")]
public class DataGridViewNumericUpDownCell : DataGridViewTextBoxCell
{
    // Default value of the DecimalPlaces property
    internal const int DATAGRIDVIEWNUMERICUPDOWNCELL_defaultDecimalPlaces = 0;

    // Default value of the Increment property
    internal const decimal DATAGRIDVIEWNUMERICUPDOWNCELL_defaultIncrement = decimal.One;

    // Default value of the Maximum property
    internal const decimal DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMaximum = (decimal)100.0;

    // Default value of the Minimum property
    internal const decimal DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMinimum = decimal.Zero;

    // Default value of the ThousandsSeparator property
    internal const bool DATAGRIDVIEWNUMERICUPDOWNCELL_defaultThousandsSeparator = false;

    private const int DATAGRIDVIEWNUMERICUPDOWNCELL_defaultRenderingBitmapHeight = 22;

    // Default dimensions of the static rendering bitmap used for the painting of the non-edited cells
    private const int DATAGRIDVIEWNUMERICUPDOWNCELL_defaultRenderingBitmapWidth = 100;

    private static readonly DataGridViewContentAlignment anyCenter =
        DataGridViewContentAlignment.TopCenter | DataGridViewContentAlignment.MiddleCenter |
        DataGridViewContentAlignment.BottomCenter;

    // Used in TranslateAlignment function
    private static readonly DataGridViewContentAlignment anyRight =
        DataGridViewContentAlignment.TopRight | DataGridViewContentAlignment.MiddleRight |
        DataGridViewContentAlignment.BottomRight;

    // Type of this cell's editing control
    private static readonly Type defaultEditType = typeof(DataGridViewNumericUpDownEditingControl);

    // Type of this cell's value. The formatted value type is string, the same as the base class DataGridViewTextBoxCell
    private static readonly Type defaultValueType = typeof(decimal);

    // The NumericUpDown control used to paint the non-edited cells via a call to NumericUpDown.DrawToBitmap
    [ThreadStatic]
    private static NumericUpDown paintingNumericUpDown;

    // The bitmap used to paint the non-edited cells via a call to NumericUpDown.DrawToBitmap
    [ThreadStatic]
    private static Bitmap renderingBitmap;

    private int decimalPlaces;

    // Caches the value of the DecimalPlaces property
    private decimal increment;

    private decimal maximum;

    // Caches the value of the Increment property
    private decimal minimum;

    // Caches the value of the Minimum property
    // Caches the value of the Maximum property
    private bool thousandsSeparator;

    /// <summary>
    /// Constructor for the DataGridViewNumericUpDownCell cell type
    /// </summary>
    public DataGridViewNumericUpDownCell()
    {
        // Create a thread specific bitmap used for the painting of the non-edited cells
        renderingBitmap ??= new Bitmap(DATAGRIDVIEWNUMERICUPDOWNCELL_defaultRenderingBitmapWidth, DATAGRIDVIEWNUMERICUPDOWNCELL_defaultRenderingBitmapHeight);

        // Create a thread specific NumericUpDown control used for the painting of the non-edited cells
        paintingNumericUpDown ??= new NumericUpDown
        {
            // Some properties only need to be set once for the lifetime of the control:
            BorderStyle = BorderStyle.None,
            Maximum = decimal.MaxValue / 10,
            Minimum = decimal.MinValue / 10
        };

        // Set the default values of the properties:
        this.decimalPlaces = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultDecimalPlaces;
        this.increment = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultIncrement;
        this.minimum = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMinimum;
        this.maximum = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultMaximum;
        this.thousandsSeparator = DATAGRIDVIEWNUMERICUPDOWNCELL_defaultThousandsSeparator;
    }

    // Caches the value of the ThousandsSeparator property
    /// <summary>
    /// The DecimalPlaces property replicates the one from the NumericUpDown control
    /// </summary>
    [
        DefaultValue(DATAGRIDVIEWNUMERICUPDOWNCELL_defaultDecimalPlaces)
    ]
    public int DecimalPlaces
    {
        get => this.decimalPlaces;

        set
        {
            if (value is < 0 or > 99)
            {
                throw new ArgumentOutOfRangeException("The DecimalPlaces property cannot be smaller than 0 or larger than 99.");
            }
            if (this.decimalPlaces != value)
            {
                SetDecimalPlaces(this.RowIndex, value);
                OnCommonChange();  // Assure that the cell or column gets repainted and autosized if needed
            }
        }
    }

    /// <summary>
    /// Define the type of the cell's editing control
    /// </summary>
    public override Type EditType => defaultEditType; // the type is DataGridViewNumericUpDownEditingControl

    /// <summary>
    /// The Increment property replicates the one from the NumericUpDown control
    /// </summary>
    public decimal Increment
    {
        get => this.increment;

        set
        {
            if (value < (decimal)0.0)
            {
                throw new ArgumentOutOfRangeException("The Increment property cannot be smaller than 0.");
            }
            SetIncrement(this.RowIndex, value);
            // No call to OnCommonChange is needed since the increment value does not affect the rendering of the cell.
        }
    }

    /// <summary>
    /// The Maximum property replicates the one from the NumericUpDown control
    /// </summary>
    public decimal Maximum
    {
        get => this.maximum;

        set
        {
            if (this.maximum != value)
            {
                SetMaximum(this.RowIndex, value);
                OnCommonChange();
            }
        }
    }

    /// <summary>
    /// The Minimum property replicates the one from the NumericUpDown control
    /// </summary>
    public decimal Minimum
    {
        get => this.minimum;

        set
        {
            if (this.minimum != value)
            {
                SetMinimum(this.RowIndex, value);
                OnCommonChange();
            }
        }
    }

    /// <summary>
    /// The ThousandsSeparator property replicates the one from the NumericUpDown control
    /// </summary>
    [
        DefaultValue(DATAGRIDVIEWNUMERICUPDOWNCELL_defaultThousandsSeparator)
    ]
    public bool ThousandsSeparator
    {
        get => this.thousandsSeparator;

        set
        {
            if (this.thousandsSeparator != value)
            {
                SetThousandsSeparator(this.RowIndex, value);
                OnCommonChange();
            }
        }
    }

    /// <summary>
    /// Returns the type of the cell's Value property
    /// </summary>
    public override Type ValueType
    {
        get
        {
            var valueType = base.ValueType;
            return valueType ?? defaultValueType;
        }
    }

    /// <summary>
    /// Returns the current DataGridView EditingControl as a DataGridViewNumericUpDownEditingControl control
    /// </summary>
    private DataGridViewNumericUpDownEditingControl EditingNumericUpDown => this.DataGridView.EditingControl as DataGridViewNumericUpDownEditingControl;

    /// <summary>
    /// Clones a DataGridViewNumericUpDownCell cell, copies all the custom properties.
    /// </summary>
    public override object Clone()
    {
        var dataGridViewCell = base.Clone() as DataGridViewNumericUpDownCell;
        if (dataGridViewCell != null)
        {
            dataGridViewCell.DecimalPlaces = this.DecimalPlaces;
            dataGridViewCell.Increment = this.Increment;
            dataGridViewCell.Maximum = this.Maximum;
            dataGridViewCell.Minimum = this.Minimum;
            dataGridViewCell.ThousandsSeparator = this.ThousandsSeparator;
        }
        return dataGridViewCell;
    }

    /// <summary>
    /// DetachEditingControl gets called by the DataGridView control when the editing session is ending
    /// </summary>
    [
        EditorBrowsable(EditorBrowsableState.Advanced)
    ]
    public override void DetachEditingControl()
    {
        var dataGridView = this.DataGridView;
        if (dataGridView == null || dataGridView.EditingControl == null)
        {
            throw new InvalidOperationException("Cell is detached or its grid has no editing control.");
        }

        if (dataGridView.EditingControl is NumericUpDown numericUpDown)
        {
            // Editing controls get recycled. Indeed, when a DataGridViewNumericUpDownCell cell gets edited
            // after another DataGridViewNumericUpDownCell cell, the same editing control gets reused for
            // performance reasons (to avoid an unnecessary control destruction and creation).
            // Here the undo buffer of the TextBox inside the NumericUpDown control gets cleared to avoid
            // interferences between the editing sessions.
            var textBox = numericUpDown.Controls[1] as TextBox;
            textBox?.ClearUndo();
        }

        base.DetachEditingControl();
    }

    /// <summary>
    /// Custom implementation of the InitializeEditingControl function. This function is called by the DataGridView control
    /// at the beginning of an editing session. It makes sure that the properties of the NumericUpDown editing control are
    /// set according to the cell properties.
    /// </summary>
    public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
    {
        base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
        if (this.DataGridView.EditingControl is NumericUpDown numericUpDown)
        {
            numericUpDown.BorderStyle = BorderStyle.None;
            numericUpDown.DecimalPlaces = this.DecimalPlaces;
            numericUpDown.Increment = this.Increment;
            numericUpDown.Maximum = this.Maximum;
            numericUpDown.Minimum = this.Minimum;
            numericUpDown.ThousandsSeparator = this.ThousandsSeparator;
            numericUpDown.Text = initialFormattedValue is not string initialFormattedValueStr ? string.Empty : initialFormattedValueStr;
        }
    }

    /// <summary>
    /// Custom implementation of the KeyEntersEditMode function. This function is called by the DataGridView control
    /// to decide whether a keystroke must start an editing session or not. In this case, a new session is started when
    /// a digit or negative sign key is hit.
    /// </summary>
    public override bool KeyEntersEditMode(KeyEventArgs e)
    {
        var numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
        var negativeSignKey = Keys.None;
        string negativeSignStr = numberFormatInfo.NegativeSign;
        if (!string.IsNullOrEmpty(negativeSignStr) && negativeSignStr.Length == 1)
        {
            negativeSignKey = (Keys)VkKeyScan(negativeSignStr[0]);
        }

        return (char.IsDigit((char)e.KeyCode) ||
             (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) ||
             negativeSignKey == e.KeyCode ||
             Keys.Subtract == e.KeyCode) &&
            !e.Shift && !e.Alt && !e.Control;
    }

    /// <summary>
    /// Custom implementation of the PositionEditingControl method called by the DataGridView control when it
    /// needs to relocate and/or resize the editing control.
    /// </summary>
    public override void PositionEditingControl(bool setLocation, bool setSize,
        Rectangle cellBounds, Rectangle cellClip, DataGridViewCellStyle cellStyle,
        bool singleVerticalBorderAdded, bool singleHorizontalBorderAdded,
        bool isFirstDisplayedColumn, bool isFirstDisplayedRow)
    {
        var editingControlBounds = PositionEditingPanel(
            cellBounds,
            cellClip,
            cellStyle,
            singleVerticalBorderAdded,
            singleHorizontalBorderAdded,
            isFirstDisplayedColumn,
            isFirstDisplayedRow);

        editingControlBounds = GetAdjustedEditingControlBounds(editingControlBounds, cellStyle);
        this.DataGridView.EditingControl.Location = new Point(editingControlBounds.X, editingControlBounds.Y);
        this.DataGridView.EditingControl.Size = new Size(editingControlBounds.Width, editingControlBounds.Height);
    }

    /// <summary>
    /// Little utility function used by both the cell and column types to translate a DataGridViewContentAlignment value into
    /// a HorizontalAlignment value.
    /// </summary>
    internal static HorizontalAlignment TranslateAlignment(DataGridViewContentAlignment align) => (align & anyRight) != 0
            ? HorizontalAlignment.Right
            : (align & anyCenter) != 0 ? HorizontalAlignment.Center : HorizontalAlignment.Left;

    /// <summary>
    /// Utility function that sets a new value for the DecimalPlaces property of the cell. This function is used by
    /// the cell and column DecimalPlaces property. The column uses this method instead of the DecimalPlaces
    /// property for performance reasons. This way the column can invalidate the entire column at once instead of
    /// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
    /// this cell may be shared among multiple rows.
    /// </summary>
    internal void SetDecimalPlaces(int rowIndex, int value)
    {
        Debug.Assert(value is >= 0 and <= 99);
        this.decimalPlaces = value;
        if (OwnsEditingNumericUpDown(rowIndex))
        {
            this.EditingNumericUpDown.DecimalPlaces = value;
        }
    }

    /// Utility function that sets a new value for the Increment property of the cell. This function is used by
    /// the cell and column Increment property. A row index needs to be provided as a parameter because
    /// this cell may be shared among multiple rows.
    internal void SetIncrement(int rowIndex, decimal value)
    {
        Debug.Assert(value >= (decimal)0.0);
        this.increment = value;
        if (OwnsEditingNumericUpDown(rowIndex))
        {
            this.EditingNumericUpDown.Increment = value;
        }
    }

    /// Utility function that sets a new value for the Maximum property of the cell. This function is used by
    /// the cell and column Maximum property. The column uses this method instead of the Maximum
    /// property for performance reasons. This way the column can invalidate the entire column at once instead of
    /// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
    /// this cell may be shared among multiple rows.
    internal void SetMaximum(int rowIndex, decimal value)
    {
        this.maximum = value;
        if (this.minimum > this.maximum)
        {
            this.minimum = this.maximum;
        }
        object cellValue = GetValue(rowIndex);
        if (cellValue != null)
        {
            decimal currentValue = Convert.ToDecimal(cellValue);
            decimal constrainedValue = Constrain(currentValue);
            if (constrainedValue != currentValue)
            {
                SetValue(rowIndex, constrainedValue);
            }
        }
        Debug.Assert(this.maximum == value);
        if (OwnsEditingNumericUpDown(rowIndex))
        {
            this.EditingNumericUpDown.Maximum = value;
        }
    }

    /// Utility function that sets a new value for the Minimum property of the cell. This function is used by
    /// the cell and column Minimum property. The column uses this method instead of the Minimum
    /// property for performance reasons. This way the column can invalidate the entire column at once instead of
    /// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
    /// this cell may be shared among multiple rows.
    internal void SetMinimum(int rowIndex, decimal value)
    {
        this.minimum = value;
        if (this.minimum > this.maximum)
        {
            this.maximum = value;
        }
        object cellValue = GetValue(rowIndex);
        if (cellValue != null)
        {
            decimal currentValue = Convert.ToDecimal(cellValue);
            decimal constrainedValue = Constrain(currentValue);
            if (constrainedValue != currentValue)
            {
                SetValue(rowIndex, constrainedValue);
            }
        }
        Debug.Assert(this.minimum == value);
        if (OwnsEditingNumericUpDown(rowIndex))
        {
            this.EditingNumericUpDown.Minimum = value;
        }
    }

    /// Utility function that sets a new value for the ThousandsSeparator property of the cell. This function is used by
    /// the cell and column ThousandsSeparator property. The column uses this method instead of the ThousandsSeparator
    /// property for performance reasons. This way the column can invalidate the entire column at once instead of
    /// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
    /// this cell may be shared among multiple rows.
    internal void SetThousandsSeparator(int rowIndex, bool value)
    {
        this.thousandsSeparator = value;
        if (OwnsEditingNumericUpDown(rowIndex))
        {
            this.EditingNumericUpDown.ThousandsSeparator = value;
        }
    }

    /// <summary>
    /// Customized implementation of the GetErrorIconBounds function in order to draw the potential
    /// error icon next to the up/down buttons and not on top of them.
    /// </summary>
    protected override Rectangle GetErrorIconBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
    {
        const int ButtonsWidth = 16;

        var errorIconBounds = base.GetErrorIconBounds(graphics, cellStyle, rowIndex);
        errorIconBounds.X = this.DataGridView.RightToLeft == RightToLeft.Yes ? errorIconBounds.Left + ButtonsWidth : errorIconBounds.Left - ButtonsWidth;
        return errorIconBounds;
    }

    /// <summary>
    /// Customized implementation of the GetFormattedValue function in order to include the decimal and thousand separator
    /// characters in the formatted representation of the cell value.
    /// </summary>
    protected override object GetFormattedValue(object value, int rowIndex,
        ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter,
        TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
    {
        // By default, the base implementation converts the decimal 1234.5 into the string "1234.5"
        object formattedValue = base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
        string formattedNumber = formattedValue as string;
        if (!string.IsNullOrEmpty(formattedNumber) && value != null)
        {
            decimal unformattedDecimal = Convert.ToDecimal(value);
            decimal formattedDecimal = Convert.ToDecimal(formattedNumber);
            if (unformattedDecimal == formattedDecimal)
            {
                // The base implementation of GetFormattedValue (which triggers the CellFormatting event) did nothing else than
                // the typical 1234.5 to "1234.5" conversion. But depending on the values of ThousandsSeparator and DecimalPlaces,
                // this may not be the actual string displayed. The real formatted value may be "1,234.500"
                return formattedDecimal.ToString((this.ThousandsSeparator ? "N" : "F") + this.DecimalPlaces.ToString());
            }
        }
        return formattedValue;
    }

    /// <summary>
    /// Custom implementation of the GetPreferredSize function. This implementation uses the preferred size of the base
    /// DataGridViewTextBoxCell cell and adds room for the up/down buttons.
    /// </summary>
    protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
    {
        if (this.DataGridView == null)
        {
            return new Size(-1, -1);
        }

        var preferredSize = base.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
        if (constraintSize.Width == 0)
        {
            const int ButtonsWidth = 16; // Account for the width of the up/down buttons.
            const int ButtonMargin = 8;  // Account for some blank pixels between the text and buttons.
            preferredSize.Width += ButtonsWidth + ButtonMargin;
        }
        return preferredSize;
    }

    /// <summary>
    /// Custom paints the cell. The base implementation of the DataGridViewTextBoxCell type is called first,
    /// dropping the icon error and content foreground parts. Those two parts are painted by this custom implementation.
    /// In this sample, the non-edited NumericUpDown control is painted by using a call to Control.DrawToBitmap. This is
    /// an easy solution for painting controls but it's not necessarily the most performant. An alternative would be to paint
    /// the NumericUpDown control piece by piece (text and up/down buttons).
    /// </summary>
    protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds,
        int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue,
        string errorText, DataGridViewCellStyle cellStyle,
        DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
    {
        if (this.DataGridView == null)
        {
            return;
        }

        // First paint the borders and background of the cell.
        base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle,
                   paintParts & ~(DataGridViewPaintParts.ErrorIcon | DataGridViewPaintParts.ContentForeground));

        var ptCurrentCell = this.DataGridView.CurrentCellAddress;
        bool cellCurrent = ptCurrentCell.X == this.ColumnIndex && ptCurrentCell.Y == rowIndex;
        bool cellEdited = cellCurrent && this.DataGridView.EditingControl != null;

        // If the cell is in editing mode, there is nothing else to paint
        if (!cellEdited)
        {
            if (PartPainted(paintParts, DataGridViewPaintParts.ContentForeground))
            {
                // Paint a NumericUpDown control
                // Take the borders into account
                var borderWidths = BorderWidths(advancedBorderStyle);
                var valBounds = cellBounds;
                valBounds.Offset(borderWidths.X, borderWidths.Y);
                valBounds.Width -= borderWidths.Right;
                valBounds.Height -= borderWidths.Bottom;
                // Also take the padding into account
                if (cellStyle.Padding != Padding.Empty)
                {
                    if (this.DataGridView.RightToLeft == RightToLeft.Yes)
                    {
                        valBounds.Offset(cellStyle.Padding.Right, cellStyle.Padding.Top);
                    }
                    else
                    {
                        valBounds.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
                    }
                    valBounds.Width -= cellStyle.Padding.Horizontal;
                    valBounds.Height -= cellStyle.Padding.Vertical;
                }
                // Determine the NumericUpDown control location
                valBounds = GetAdjustedEditingControlBounds(valBounds, cellStyle);

                bool cellSelected = (cellState & DataGridViewElementStates.Selected) != 0;

                if (renderingBitmap.Width < valBounds.Width ||
                    renderingBitmap.Height < valBounds.Height)
                {
                    // The static bitmap is too small, a bigger one needs to be allocated.
                    renderingBitmap.Dispose();
                    renderingBitmap = new Bitmap(valBounds.Width, valBounds.Height);
                }
                // Make sure the NumericUpDown control is parented to a visible control
                if (paintingNumericUpDown.Parent == null || !paintingNumericUpDown.Parent.Visible)
                {
                    paintingNumericUpDown.Parent = this.DataGridView;
                }
                // Set all the relevant properties
                paintingNumericUpDown.TextAlign = DataGridViewNumericUpDownCell.TranslateAlignment(cellStyle.Alignment);
                paintingNumericUpDown.DecimalPlaces = this.DecimalPlaces;
                paintingNumericUpDown.ThousandsSeparator = this.ThousandsSeparator;
                paintingNumericUpDown.Font = cellStyle.Font;
                paintingNumericUpDown.Width = valBounds.Width;
                paintingNumericUpDown.Height = valBounds.Height;
                paintingNumericUpDown.RightToLeft = this.DataGridView.RightToLeft;
                paintingNumericUpDown.Location = new Point(0, -paintingNumericUpDown.Height - 100);
                paintingNumericUpDown.Text = formattedValue as string;

                var backColor = PartPainted(paintParts, DataGridViewPaintParts.SelectionBackground) && cellSelected
                    ? cellStyle.SelectionBackColor
                    : cellStyle.BackColor;
                if (PartPainted(paintParts, DataGridViewPaintParts.Background))
                {
                    if (backColor.A < 255)
                    {
                        // The NumericUpDown control does not support transparent back colors
                        backColor = Color.FromArgb(255, backColor);
                    }
                    paintingNumericUpDown.BackColor = backColor;
                }
                // Finally paint the NumericUpDown control
                var srcRect = new Rectangle(0, 0, valBounds.Width, valBounds.Height);
                if (srcRect.Width > 0 && srcRect.Height > 0)
                {
                    paintingNumericUpDown.DrawToBitmap(renderingBitmap, srcRect);
                    graphics.DrawImage(renderingBitmap, new Rectangle(valBounds.Location, valBounds.Size),
                                       srcRect, GraphicsUnit.Pixel);
                }
            }
            if (PartPainted(paintParts, DataGridViewPaintParts.ErrorIcon))
            {
                // Paint the potential error icon on top of the NumericUpDown control
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText,
                           cellStyle, advancedBorderStyle, DataGridViewPaintParts.ErrorIcon);
            }
        }
    }

    /// <summary>
    /// Little utility function called by the Paint function to see if a particular part needs to be painted.
    /// </summary>
    private static bool PartPainted(DataGridViewPaintParts paintParts, DataGridViewPaintParts paintPart) => (paintParts & paintPart) != 0;

    // Used in KeyEntersEditMode function
    [System.Runtime.InteropServices.DllImport("USER32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    private static extern short VkKeyScan(char key);

    /// <summary>
    /// Returns the provided value constrained to be within the min and max.
    /// </summary>
    private decimal Constrain(decimal value)
    {
        Debug.Assert(this.minimum <= this.maximum);
        if (value < this.minimum)
        {
            value = this.minimum;
        }
        if (value > this.maximum)
        {
            value = this.maximum;
        }
        return value;
    }

    /// <summary>
    /// Adjusts the location and size of the editing control given the alignment characteristics of the cell
    /// </summary>
    private Rectangle GetAdjustedEditingControlBounds(Rectangle editingControlBounds, DataGridViewCellStyle cellStyle)
    {
        // Add a 1 pixel padding on the left and right of the editing control
        editingControlBounds.X += 1;
        editingControlBounds.Width = Math.Max(0, editingControlBounds.Width - 2);

        // Adjust the vertical location of the editing control:
        int preferredHeight = cellStyle.Font.Height + 3;
        if (preferredHeight < editingControlBounds.Height)
        {
            switch (cellStyle.Alignment)
            {
                case DataGridViewContentAlignment.MiddleLeft:
                case DataGridViewContentAlignment.MiddleCenter:
                case DataGridViewContentAlignment.MiddleRight:
                    editingControlBounds.Y += (editingControlBounds.Height - preferredHeight) / 2;
                    break;

                case DataGridViewContentAlignment.BottomLeft:
                case DataGridViewContentAlignment.BottomCenter:
                case DataGridViewContentAlignment.BottomRight:
                    editingControlBounds.Y += editingControlBounds.Height - preferredHeight;
                    break;
            }
        }

        return editingControlBounds;
    }

    /// <summary>
    /// Called when a cell characteristic that affects its rendering and/or preferred size has changed.
    /// This implementation only takes care of repainting the cells. The DataGridView's autosizing methods
    /// also need to be called in cases where some grid elements autosize.
    /// </summary>
    private void OnCommonChange()
    {
        if (this.DataGridView != null && !this.DataGridView.IsDisposed && !this.DataGridView.Disposing)
        {
            if (this.RowIndex == -1)
            {
                // Invalidate and autosize column
                this.DataGridView.InvalidateColumn(this.ColumnIndex);

                // TODO: Add code to autosize the cell's column, the rows, the column headers
                // and the row headers depending on their autosize settings.
                // The DataGridView control does not expose a public method that takes care of this.
            }
            else
            {
                // The DataGridView control exposes a public method called UpdateCellValue
                // that invalidates the cell so that it gets repainted and also triggers all
                // the necessary autosizing: the cell's column and/or row, the column headers
                // and the row headers are autosized depending on their autosize settings.
                this.DataGridView.UpdateCellValue(this.ColumnIndex, this.RowIndex);
            }
        }
    }

    /// <summary>
    /// Determines whether this cell, at the given row index, shows the grid's editing control or not.
    /// The row index needs to be provided as a parameter because this cell may be shared among multiple rows.
    /// </summary>
    private bool OwnsEditingNumericUpDown(int rowIndex) =>
        rowIndex != -1 &&
        this.DataGridView != null &&
        this.DataGridView.EditingControl is DataGridViewNumericUpDownEditingControl numericUpDownEditingControl &&
        rowIndex == ((IDataGridViewEditingControl)numericUpDownEditingControl).EditingControlRowIndex;
}
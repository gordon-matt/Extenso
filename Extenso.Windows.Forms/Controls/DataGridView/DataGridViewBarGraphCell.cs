namespace Extenso.Windows.Forms.Controls;

public class DataGridViewBarGraphCell : DataGridViewTextBoxCell
{
    protected override void Paint(
        Graphics graphics,
        Rectangle clipBounds,
        Rectangle cellBounds,
        int rowIndex,
        DataGridViewElementStates cellState,
        object value,
        object formattedValue,
        string errorText,
        DataGridViewCellStyle cellStyle,
        DataGridViewAdvancedBorderStyle advancedBorderStyle,
        DataGridViewPaintParts paintParts)
    {
        base.Paint(
            graphics, clipBounds, cellBounds, rowIndex, cellState, value,
            string.Empty, errorText, cellStyle, advancedBorderStyle, paintParts);

        //  Get the value of the cell:
        decimal cellValue;

        try
        {
            if (Convert.IsDBNull(value))
            { cellValue = 0; }
            else
            { cellValue = Convert.ToDecimal(value); }
        }
        catch { cellValue = 0; }

        //  If cell value is 0, you still
        //  want to show something, so set the value
        //  to 1.
        if (cellValue == 0) { cellValue = 1; }

        const int HORIZONTALOFFSET = 1;
        const int SPACER = 4;

        //  Get the parent column and the maximum value:
        var parent = (DataGridViewBarGraphColumn)this.OwningColumn;

        // Calculate the maximum value in the parent
        // column. This code only runs once, but it
        // needs to be called from a location in which
        // you know the data binding has completed:
        parent.CalcMaxValue();
        long maxValue = parent.MaxValue;

        var fnt = parent.InheritedStyle.Font;
        var maxValueSize = graphics.MeasureString(maxValue.ToString(), fnt);
        float availableWidth = cellBounds.Width - maxValueSize.Width - SPACER - (HORIZONTALOFFSET * 2);
        cellValue = Convert.ToDecimal((Convert.ToDouble(cellValue) / maxValue) * availableWidth);

        const int VERTOFFSET = 4;
        var newRect = new RectangleF(cellBounds.X + HORIZONTALOFFSET, cellBounds.Y + VERTOFFSET, Convert.ToSingle(cellValue), cellBounds.Height - (VERTOFFSET * 2));
        graphics.FillRectangle(Brushes.Red, newRect);

        string cellText = formattedValue.ToString();
        var textSize = graphics.MeasureString(cellText, fnt);

        //  Calculate where text would start:
        var textStart = new PointF(
            Convert.ToSingle(
                HORIZONTALOFFSET + cellValue + SPACER),
                (cellBounds.Height - textSize.Height) / 2);

        //  Calculate the correct color:
        var textColor = parent.InheritedStyle.ForeColor;
        if ((cellState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
        { textColor = parent.InheritedStyle.SelectionForeColor; }

        // Draw the text:
        using var brush = new SolidBrush(textColor);
        graphics.DrawString(cellText, fnt, brush, cellBounds.X + textStart.X, cellBounds.Y + textStart.Y);
    }
}
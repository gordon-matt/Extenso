using System.ComponentModel;
using System.Globalization;

namespace Extenso.Windows.Forms.Controls;

[ToolboxBitmap(typeof(TextBox)),
ToolboxItem(true),
Description("Custom TextBox, Accepts Numerical Input Only")]
public partial class NumericTextBox : TextBox
{
    public bool AllowSpace { set; get; } = false;

    public decimal DecimalValue => decimal.Parse(Text);

    public int IntValue => int.Parse(Text);

    // Restricts the entry of characters to digits (including hex), the negative sign,
    // the decimal point, and editing keystrokes (backspace).
    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        base.OnKeyPress(e);

        var numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
        string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
        string groupSeparator = numberFormatInfo.NumberGroupSeparator;
        string negativeSign = numberFormatInfo.NegativeSign;

        string keyInput = e.KeyChar.ToString();

        if (char.IsDigit(e.KeyChar))
        {
            // Digits are OK
        }
        else if (keyInput.Equals(decimalSeparator) || keyInput.Equals(groupSeparator) || keyInput.Equals(negativeSign))
        {
            // Decimal separator is OK
        }
        else if (e.KeyChar == '\b')
        {
            // Backspace key is OK
        }
        else if ((ModifierKeys & (Keys.Control | Keys.Alt)) != 0)
        {
            // Let the edit control handle control and alt key combinations
        }
        else if (AllowSpace && e.KeyChar == ' ')
        {
        }
        else
        {
            // Consume this invalid key and beep
            e.Handled = true;
            //    MessageBeep();
        }
    }
}
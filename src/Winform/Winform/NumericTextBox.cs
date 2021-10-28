using System.Globalization;
using System.Windows.Forms;

namespace VectronsLibrary.Winform;

/// <summary>
/// A numeric only <see cref="TextBox"/>.
/// </summary>
public class NumericTextBox : TextBox
{
    /// <summary>
    /// Gets or sets a value indicating whether decimal values are allowed in the <see cref="NumericTextBox"/>.
    /// </summary>
    public bool AllowdecimalSeparator
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether group seperators are allowed in the <see cref="NumericTextBox"/>.
    /// </summary>
    public bool AllowgroupSeparator
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether negative values are allowed in the <see cref="NumericTextBox"/>.
    /// </summary>
    public bool AllownegativeSign
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether spaces are allowed in the <see cref="NumericTextBox"/>.
    /// </summary>
    public bool AllowSpace
    {
        get;
        set;
    }

    /// <summary>
    /// Gets the current value as a <c>Decimal</c>.
    /// </summary>
    public decimal DecimalValue
    {
        get
        {
            _ = decimal.TryParse(Text, out var result);
            return result;
        }
    }

    /// <summary>
    /// Gets the current value as a <c>int</c>.
    /// </summary>
    public int IntValue
    {
        get
        {
            _ = int.TryParse(Text, out var result);
            return result;
        }
    }

    /// <summary>
    /// Restricts the entry of characters to digits (including hex), the negative sign, the decimal point, and editing keystrokes (backspace).
    /// </summary>
    /// <param name="e">The <see cref="KeyPressEventArgs"/>.</param>
    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        base.OnKeyPress(e);

        var numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
        var decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
        var groupSeparator = numberFormatInfo.NumberGroupSeparator;
        var negativeSign = numberFormatInfo.NegativeSign;

        // Workaround for groupSeparator equal to non-breaking space
        if (groupSeparator == ((char)160).ToString())
        {
            groupSeparator = " ";
        }

        var keyInput = e.KeyChar.ToString();

        if (char.IsDigit(e.KeyChar))
        {
            // Digits are OK
        }
        else if (keyInput.Equals(decimalSeparator, System.StringComparison.Ordinal) && AllowdecimalSeparator)
        {
            // Decimal separator is OK
        }
        else if (keyInput.Equals(groupSeparator, System.StringComparison.Ordinal) && AllowgroupSeparator)
        {
            // Group separator is OK
        }
        else if (keyInput.Equals(negativeSign, System.StringComparison.Ordinal) && AllownegativeSign)
        {
            // Negative Sign is OK
        }
        else if (e.KeyChar == '\b')
        {
            // Backspace key is OK
        }
        else if (AllowSpace && e.KeyChar == ' ')
        {
            // Space key is OK
        }
        else
        {
            // Consume this invalid key
            e.Handled = true;
        }
    }
}
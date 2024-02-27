using System.Globalization;
using System.Windows.Forms;

namespace VectronsLibrary.WindowsForms;

/// <summary>
/// A numeric only <see cref="TextBox"/>.
/// </summary>
public class NumericTextBox : TextBox
{
    /// <summary>
    /// Gets or sets a value indicating whether decimal values are allowed in the <see cref="NumericTextBox"/>.
    /// </summary>
    public bool AllowDecimalSeparator
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether group separators are allowed in the <see cref="NumericTextBox"/>.
    /// </summary>
    public bool AllowGroupSeparator
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether negative values are allowed in the <see cref="NumericTextBox"/>.
    /// </summary>
    public bool AllowNegativeSign
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
    /// Gets the current value as a <see langword="decimal"/>.
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
    /// Gets the current value as a <see langword="int"/>.
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
        if (string.Equals(groupSeparator, ((char)160).ToString(), System.StringComparison.Ordinal))
        {
            groupSeparator = " ";
        }

        var keyInput = e.KeyChar.ToString();

        if (char.IsDigit(e.KeyChar))
        {
            // Digits are OK
        }
        else if (keyInput.Equals(decimalSeparator, System.StringComparison.Ordinal) && AllowDecimalSeparator)
        {
            // Decimal separator is OK
        }
        else if (keyInput.Equals(groupSeparator, System.StringComparison.Ordinal) && AllowGroupSeparator)
        {
            // Group separator is OK
        }
        else if (keyInput.Equals(negativeSign, System.StringComparison.Ordinal) && AllowNegativeSign)
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

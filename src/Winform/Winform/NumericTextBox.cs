using System.Globalization;
using System.Windows.Forms;

namespace VectronsLibrary.Winform
{
    public class NumericTextBox : TextBox
    {
        public bool AllowdecimalSeparator
        {
            get; set;
        }

        public bool AllowgroupSeparator
        {
            get; set;
        }

        public bool AllownegativeSign
        {
            get; set;
        }

        public bool AllowSpace
        {
            get; set;
        }

        public decimal DecimalValue
        {
            get
            {
                decimal.TryParse(Text, out decimal result);
                return result;
            }
        }

        public int IntValue
        {
            get
            {
                int.TryParse(Text, out int result);
                return result;
            }
        }

        // Restricts the entry of characters to digits (including hex), the negative sign,
        // the decimal point, and editing keystrokes (backspace).
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            NumberFormatInfo numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string groupSeparator = numberFormatInfo.NumberGroupSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;

            // Workaround for groupSeparator equal to non-breaking space
            if (groupSeparator == ((char)160).ToString())
            {
                groupSeparator = " ";
            }

            string keyInput = e.KeyChar.ToString();

            if (char.IsDigit(e.KeyChar))
            {
                // Digits are OK
            }
            else if (keyInput.Equals(decimalSeparator) && AllowdecimalSeparator)
            {
                // Decimal separator is OK
            }
            else if (keyInput.Equals(groupSeparator) && AllowgroupSeparator)
            {
                // Group separator is OK
            }
            else if (keyInput.Equals(negativeSign) && AllownegativeSign)
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
}
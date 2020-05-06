using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VectronsLibrary.Wpf.Controlls
{
    public class NumericTextBox : TextBox
    {
        public static readonly DependencyProperty DecimalProperty = DependencyProperty
            .Register(nameof(AllowDecimal), typeof(bool), typeof(NumericTextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MinusProperty = DependencyProperty
            .Register(nameof(AllowNegative), typeof(bool), typeof(NumericTextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        static NumericTextBox()
        {
            DataObjectPastingEventHandler handler = (sender, e) =>
            {
                if (!IsDataValid(e.DataObject))
                {
                    var data = new DataObject();
                    data.SetText(string.Empty);
                    e.DataObject = data;
                    e.Handled = false;
                }
            };
            EventManager.RegisterClassHandler(typeof(NumericTextBox), DataObject.PastingEvent, handler);
        }

        public bool AllowDecimal
        {
            get => (bool)GetValue(DecimalProperty);
            set => SetValue(DecimalProperty, value);
        }

        public bool AllowNegative
        {
            get => (bool)GetValue(MinusProperty);
            set => SetValue(MinusProperty, value);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            if (!IsDataValid(e.Data))
            {
                e.Handled = true;
                e.Effects = DragDropEffects.None;
            }

            OnDragEnter(e);
        }

        protected override void OnDrop(DragEventArgs e)
        {
            e.Handled = !IsDataValid(e.Data);
            base.OnDrop(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((e.Key < Key.D0 || e.Key > Key.D9) // Key is not a decimal key
                && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) // Key is not a numpad decimal key
                && (e.Key != Key.Back) // key is not backspace
                && (!AllowDecimal || (AllowDecimal && e.Key != Key.OemComma)) // Decimal numbers are not allowed or key is not the comma key
                && (!AllowNegative || (AllowNegative && CaretIndex != 0 && (e.Key != Key.Subtract || e.Key != Key.OemMinus))) // minus numbers are not allowed or key is not the minus key
                && (e.Key != Key.Tab))
            {
                e.Handled = true;
            }
        }

        private static bool IsDataValid(IDataObject data)
        {
            bool isValid = false;
            if (data != null)
            {
                string text = data.GetData(DataFormats.Text) as string;

                if (!string.IsNullOrEmpty(text?.Trim()))
                {
                    if (int.TryParse(text, out int intResult))
                    {
                        isValid = true;
                    }
                    else if (double.TryParse(text, out double doubleResult))
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }
    }
}
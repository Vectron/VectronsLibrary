using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VectronsLibrary.Wpf.Controlls
{
    /// <summary>
    /// A numeric only <see cref="TextBox"/>.
    /// </summary>
    public class NumericTextBox : TextBox
    {
        /// <summary>
        /// Identifies the <see cref="Label"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AllowDecimalProperty = DependencyProperty
            .Register(nameof(AllowDecimal), typeof(bool), typeof(NumericTextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Identifies the <see cref="Label"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AllowNegativeProperty = DependencyProperty
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

        /// <summary>
        /// Gets or sets a value indicating whether decimal values are allowed in the <see cref="NumericTextBox"/>.
        /// </summary>
        public bool AllowDecimal
        {
            get => (bool)GetValue(AllowDecimalProperty);
            set => SetValue(AllowDecimalProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether negative values are allowed in the <see cref="NumericTextBox"/>.
        /// </summary>
        public bool AllowNegative
        {
            get => (bool)GetValue(AllowNegativeProperty);
            set => SetValue(AllowNegativeProperty, value);
        }

        /// <inheritdoc/>
        protected override void OnDragOver(DragEventArgs e)
        {
            if (!IsDataValid(e.Data))
            {
                e.Handled = true;
                e.Effects = DragDropEffects.None;
            }

            OnDragEnter(e);
        }

        /// <inheritdoc/>
        protected override void OnDrop(DragEventArgs e)
        {
            e.Handled = !IsDataValid(e.Data);
            base.OnDrop(e);
        }

        /// <inheritdoc/>
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
            var isValid = false;
            if (data != null)
            {
                var text = data.GetData(DataFormats.Text) as string;

                if (!string.IsNullOrEmpty(text?.Trim()))
                {
                    if (int.TryParse(text, out _))
                    {
                        isValid = true;
                    }
                    else if (double.TryParse(text, out _))
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }
    }
}
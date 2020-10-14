using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VectronsLibrary.Wpf.Controlls
{
    [TemplatePart(Name = IPInput.ElementFirstIPPartTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = IPInput.ElementSecondIPPartTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = IPInput.ElementThirdIPPartTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = IPInput.ElementFourthIPPartTextBox, Type = typeof(TextBox))]
    public class IPInput : Control
    {
        public static readonly DependencyProperty IPAddressProperty = DependencyProperty
            .Register(nameof(IPAddress), typeof(string), typeof(IPInput), new FrameworkPropertyMetadata("127.0.0.1", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IpAdressChanged));

        private const string ElementFirstIPPartTextBox = "PART_FirstIPPartTextBox";
        private const string ElementFourthIPPartTextBox = "PART_FourthIPPartTextBox";
        private const string ElementSecondIPPartTextBox = "PART_SecondIPPartTextBox";
        private const string ElementThirdIPPartTextBox = "PART_ThirdIPPartTextBox";
        private TextBox firstIPPartTextBox;
        private TextBox fourthIPPartTextBox;
        private TextBox secondIPPartTextBox;
        private TextBox thirdIPPartTextBox;

        static IPInput()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IPInput), new FrameworkPropertyMetadata(typeof(IPInput)));
        }

        public string IPAddress
        {
            get => (string)GetValue(IPAddressProperty);
            set => SetValue(IPAddressProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            firstIPPartTextBox = GetTemplateChild(ElementFirstIPPartTextBox) as TextBox;
            secondIPPartTextBox = GetTemplateChild(ElementSecondIPPartTextBox) as TextBox;
            thirdIPPartTextBox = GetTemplateChild(ElementThirdIPPartTextBox) as TextBox;
            fourthIPPartTextBox = GetTemplateChild(ElementFourthIPPartTextBox) as TextBox;
            UpdateTextBoxes();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e is null)
            {
                throw new System.ArgumentNullException(nameof(e));
            }

            var frameworkElement = e.OriginalSource as FrameworkElement;

            if (!(frameworkElement is TextBox textBox))
            {
                return;
            }

            if (textBox.Text.Length == 3
                && ((e.Key >= Key.D0 && e.Key <= Key.D9)
                    || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
                _ = frameworkElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }

            IPAddress = $"{firstIPPartTextBox.Text}.{secondIPPartTextBox.Text}.{thirdIPPartTextBox.Text}.{fourthIPPartTextBox.Text}";
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e is null)
            {
                throw new System.ArgumentNullException(nameof(e));
            }
            var frameworkElement = e.OriginalSource as FrameworkElement;

            if (!(frameworkElement is TextBox textBox))
            {
                return;
            }

            if (e.Key == Key.Decimal)
            {
                e.Handled = true;
                _ = frameworkElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
            else if (e.Key == Key.Back && textBox.Text.Length == 0)
            {
                e.Handled = true;
                _ = frameworkElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            }
        }

        private static void IpAdressChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (dependencyObject is IPInput ipInput)
            {
                ipInput.UpdateTextBoxes();
            }
        }

        private void UpdateTextBoxes()
        {
            if (firstIPPartTextBox == null || secondIPPartTextBox == null || thirdIPPartTextBox == null || fourthIPPartTextBox == null)
            {
                return;
            }

            var parts = IPAddress.Split('.');
            firstIPPartTextBox.Text = parts.Length >= 0 ? parts[0] : "0";
            secondIPPartTextBox.Text = parts.Length >= 1 ? parts[1] : "0";
            thirdIPPartTextBox.Text = parts.Length >= 2 ? parts[2] : "0";
            fourthIPPartTextBox.Text = parts.Length >= 3 ? parts[3] : "0";
        }
    }
}
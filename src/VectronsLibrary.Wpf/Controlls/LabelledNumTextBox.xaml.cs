using System.Windows;
using System.Windows.Controls;

namespace VectronsLibrary.Wpf.Controlls
{
    public partial class LabelledNumTextBox : UserControl
    {
        public static readonly DependencyProperty DecimalProperty = DependencyProperty
            .Register(nameof(AllowDecimal), typeof(bool), typeof(LabelledNumTextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty LabelProperty = DependencyProperty
            .Register(nameof(Label), typeof(string), typeof(LabelledNumTextBox), new FrameworkPropertyMetadata("Unnamed Label"));

        public static readonly DependencyProperty MinusProperty = DependencyProperty
            .Register(nameof(AllowNegative), typeof(bool), typeof(LabelledNumTextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty TextProperty = DependencyProperty
            .Register(nameof(Text), typeof(string), typeof(LabelledNumTextBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public LabelledNumTextBox()
        {
            InitializeComponent();
            Root.DataContext = this;
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

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}
using System.Windows;
using System.Windows.Controls;

namespace VectronsLibrary.Wpf.Controlls
{
    public partial class LabelledTextBox : UserControl
    {
        public static readonly DependencyProperty LabelProperty = DependencyProperty
            .Register(nameof(Label), typeof(string), typeof(LabelledTextBox), new FrameworkPropertyMetadata("Unnamed Label"));

        public static readonly DependencyProperty TextProperty = DependencyProperty
            .Register(nameof(Text), typeof(string), typeof(LabelledTextBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public LabelledTextBox()
        {
            InitializeComponent();
            Root.DataContext = this;
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
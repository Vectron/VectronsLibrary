using System.Windows;
using System.Windows.Controls;

namespace VectronsLibrary.Wpf.Controlls
{
    public partial class LabelledCheckBox : UserControl
    {
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty
            .Register(nameof(IsChecked), typeof(bool), typeof(LabelledCheckBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty LabelProperty = DependencyProperty
            .Register(nameof(Label), typeof(string), typeof(LabelledCheckBox), new FrameworkPropertyMetadata("Unnamed Label"));

        public LabelledCheckBox()
        {
            InitializeComponent();
            Root.DataContext = this;
        }

        public string IsChecked
        {
            get => (string)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
    }
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VectronsLibrary.Wpf.Controlls
{
    /// <summary>
    /// Interaction logic for LabelledColorPicker.xaml
    /// </summary>
    public partial class LabelledColorPicker : UserControl
    {
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
            nameof(Label),
            typeof(string),
            typeof(LabelledColorPicker),
            new FrameworkPropertyMetadata("Unnamed Label"));

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(Color),
                typeof(LabelledColorPicker),
                new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public LabelledColorPicker()
        {
            InitializeComponent();
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public string SelectedColor
        {
            get => (string)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }
    }
}
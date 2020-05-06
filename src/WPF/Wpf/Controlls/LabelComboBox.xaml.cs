using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace VectronsLibrary.Wpf.Controlls
{
    public partial class LabelComboBox : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty
            .Register(nameof(ItemsSource), typeof(IEnumerable), typeof(LabelComboBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty LabelProperty = DependencyProperty
            .Register(nameof(Label), typeof(string), typeof(LabelComboBox), new FrameworkPropertyMetadata("Unnamed Label"));

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty
            .Register(nameof(SelectedItem), typeof(object), typeof(LabelComboBox), new FrameworkPropertyMetadata(new object(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public LabelComboBox()
        {
            InitializeComponent();
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public string SelectedItem
        {
            get => (string)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
    }
}
using System.Windows;
using System.Windows.Controls;

namespace VectronsLibrary.Wpf.Controlls
{
    public partial class LabelledSlider : UserControl
    {
        public static readonly DependencyProperty LabelProperty = DependencyProperty
            .Register(nameof(Label), typeof(string), typeof(LabelledSlider), new FrameworkPropertyMetadata("Unnamed Label"));

        public static readonly DependencyProperty LargeChangeProperty = DependencyProperty
            .Register(nameof(LargeChange), typeof(int), typeof(LabelledSlider), new FrameworkPropertyMetadata(10, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty
            .Register(nameof(Maximum), typeof(int), typeof(LabelledSlider), new FrameworkPropertyMetadata(10, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MinimumProperty = DependencyProperty
            .Register(nameof(Minimum), typeof(int), typeof(LabelledSlider), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SmallChangeProperty = DependencyProperty
            .Register(nameof(SmallChange), typeof(int), typeof(LabelledSlider), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ValueProperty = DependencyProperty
            .Register(nameof(Value), typeof(int), typeof(LabelledSlider), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public LabelledSlider()
        {
            InitializeComponent();
        }

        public bool AllowNegative => Minimum < 0;

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public int LargeChange
        {
            get => (int)GetValue(LargeChangeProperty);
            set => SetValue(LargeChangeProperty, value);
        }

        public int Maximum
        {
            get => (int)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public int Minimum
        {
            get => (int)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public int SmallChange
        {
            get => (int)GetValue(SmallChangeProperty);
            set => SetValue(SmallChangeProperty, value);
        }

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
    }
}
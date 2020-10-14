using System.Windows;
using System.Windows.Controls;

namespace VectronsLibrary.Wpf.Controlls
{
    public class LabeledControll : ContentControl
    {
        public static readonly DependencyProperty LabelProperty = DependencyProperty
            .Register(nameof(Label), typeof(string), typeof(LabeledControll), new FrameworkPropertyMetadata("Unnamed Label"));

        static LabeledControll()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabeledControll), new FrameworkPropertyMetadata(typeof(LabeledControll)));
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
    }
}
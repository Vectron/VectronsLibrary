using System.Windows;
using System.Windows.Controls;

namespace VectronsLibrary.Wpf.Controlls
{
    /// <summary>
    /// Control container that adds a label infront of the control.
    /// </summary>
    public class LabeledControll : ContentControl
    {
        /// <summary>
        /// Identifies the <see cref="Label"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty
            .Register(nameof(Label), typeof(string), typeof(LabeledControll), new FrameworkPropertyMetadata("Unnamed Label"));

        static LabeledControll()
            => DefaultStyleKeyProperty.OverrideMetadata(typeof(LabeledControll), new FrameworkPropertyMetadata(typeof(LabeledControll)));

        /// <summary>
        /// Gets or sets the text of the label.
        /// </summary>
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
    }
}
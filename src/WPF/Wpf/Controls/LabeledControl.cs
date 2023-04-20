using System.Windows;
using System.Windows.Controls;

namespace VectronsLibrary.Wpf.Controls;

/// <summary>
/// Control container that adds a label in front of the control.
/// </summary>
public class LabeledControl : ContentControl
{
    /// <summary>
    /// Identifies the <see cref="Label"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LabelProperty = DependencyProperty
        .Register(nameof(Label), typeof(string), typeof(LabeledControl), new FrameworkPropertyMetadata("Unnamed Label"));

    static LabeledControl()
        => DefaultStyleKeyProperty.OverrideMetadata(typeof(LabeledControl), new FrameworkPropertyMetadata(typeof(LabeledControl)));

    /// <summary>
    /// Gets or sets the text of the label.
    /// </summary>
    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }
}
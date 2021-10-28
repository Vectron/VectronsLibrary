using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace VectronsLibrary.Wpf.Controlls;

/// <summary>
/// Interaction logic for ColorPicker.xaml.
/// </summary>
public partial class ColorPicker : UserControl
{
    /// <summary>
    /// Identifies the <see cref="SelectedColor"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedColorProperty =
        DependencyProperty.Register(
            nameof(SelectedColor),
            typeof(Color),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorPicker"/> class.
    /// </summary>
    public ColorPicker()
        => InitializeComponent();

    /// <summary>
    /// Event when the selected value changed.
    /// </summary>
    public event SelectionChangedEventHandler SelectionChanged
    {
        add
        {
            colorCombobox.AddHandler(Selector.SelectionChangedEvent, value);
        }

        remove
        {
            colorCombobox.RemoveHandler(Selector.SelectionChangedEvent, value);
        }
    }

    /// <summary>
    /// Gets a <see cref="IEnumerable{T}"/> off all known colors.
    /// </summary>
    public static IEnumerable<string> ColorPickerColors
        => typeof(Colors)
            .GetProperties(BindingFlags.Static | BindingFlags.Public)
            .Select(p => p.Name);

    /// <summary>
    /// Gets or sets the selected color.
    /// </summary>
    public Color SelectedColor
    {
        get => (Color)GetValue(SelectedColorProperty);
        set => SetValue(SelectedColorProperty, value);
    }
}
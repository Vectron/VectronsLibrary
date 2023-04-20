using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VectronsLibrary.Wpf.Controls;

/// <summary>
/// ColorSelector control.
/// </summary>
public class ColorSelector : ComboBox
{
    /// <summary>
    /// Identifies the <see cref="SelectedColor"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedColorProperty =
        DependencyProperty.Register(
            nameof(SelectedColor),
            typeof(Color),
            typeof(ColorSelector),
            new FrameworkPropertyMetadata(
                Colors.Black,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnSelectedColorChanged)));

    private static readonly NamedColor[] KnownColors = typeof(Colors)
            .GetProperties(BindingFlags.Static | BindingFlags.Public)
            .Select(p =>
            {
                var c = p.GetValue(null);
                if (c is not null and Color color)
                {
                    return new NamedColor(p.Name, color);
                }

                return new NamedColor(p.Name, default);
            }).ToArray();

    static ColorSelector()
            => DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorSelector), new FrameworkPropertyMetadata(typeof(ColorSelector)));

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSelector"/> class.
    /// </summary>
    public ColorSelector()
        : base()
    {
        ItemsSource = KnownColors;
        SelectedItem = KnownColors.FirstOrDefault(x => x.Name == "Black");
    }

    /// <summary>
    /// Gets or sets the selected color.
    /// </summary>
    public Color SelectedColor
    {
        get => (Color)GetValue(SelectedColorProperty);
        set => SetValue(SelectedColorProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnSelectionChanged(SelectionChangedEventArgs e)
    {
        base.OnSelectionChanged(e);
        if (SelectedItem is NamedColor namedColor
            && SelectedColor != namedColor.Color)
        {
            SelectedColor = namedColor.Color;
        }
    }

    /// <summary>
    /// This could happen when SelectedValuePath has changed,
    /// SelectedItem has changed, or someone is setting SelectedValue.
    /// </summary>
    private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ColorSelector colorSelector
            && colorSelector.SelectedItem is NamedColor namedColor
            && e.NewValue is Color color
            && color != namedColor.Color)
        {
            var newNamedColor = KnownColors.FirstOrDefault(x => x.Color == color);
            if (newNamedColor != null)
            {
                colorSelector.SelectedItem = newNamedColor;
            }
        }
    }
}

internal sealed record NamedColor(string Name, Color Color);
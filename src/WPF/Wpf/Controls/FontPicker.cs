using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VectronsLibrary.Wpf.Controls;

/// <summary>
/// FontPicker control.
/// </summary>
public class FontPicker : Control
{
    /// <summary>
    /// Identifies the <see cref="SelectedFontColor"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedFontColorProperty =
        DependencyProperty.Register(
            nameof(SelectedFontColor),
            typeof(Color),
            typeof(FontPicker),
            new PropertyMetadata((ForegroundProperty.DefaultMetadata.DefaultValue as SolidColorBrush)?.Color ?? default));

    /// <summary>
    /// Identifies the <see cref="SelectedFontFamily"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedFontFamilyProperty =
        DependencyProperty.Register(
            nameof(SelectedFontFamily),
            typeof(FontFamily),
            typeof(FontPicker),
            new PropertyMetadata(FontFamilyProperty.DefaultMetadata.DefaultValue));

    /// <summary>
    /// Identifies the <see cref="SelectedFontSize"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedFontSizeProperty =
        DependencyProperty.Register(
            nameof(SelectedFontSize),
            typeof(double),
            typeof(FontPicker),
            new PropertyMetadata(FontSizeProperty.DefaultMetadata.DefaultValue));

    /// <summary>
    /// Identifies the <see cref="SelectedFontStretch"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedFontStretchProperty =
        DependencyProperty.Register(
            nameof(SelectedFontStretch),
            typeof(FontStretch),
            typeof(FontPicker),
            new PropertyMetadata(FontStretchProperty.DefaultMetadata.DefaultValue));

    /// <summary>
    /// Identifies the <see cref="SelectedFontStyle"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedFontStyleProperty =
        DependencyProperty.Register(
            nameof(SelectedFontStyle),
            typeof(FontStyle),
            typeof(FontPicker),
            new PropertyMetadata(FontStyleProperty.DefaultMetadata.DefaultValue));

    /// <summary>
    /// Identifies the <see cref="SelectedFontWeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SelectedFontWeightProperty =
        DependencyProperty.Register(
            nameof(SelectedFontWeight),
            typeof(FontWeight),
            typeof(FontPicker),
            new PropertyMetadata(FontWeightProperty.DefaultMetadata.DefaultValue));

    static FontPicker()
        => DefaultStyleKeyProperty.OverrideMetadata(typeof(FontPicker), new FrameworkPropertyMetadata(typeof(FontPicker)));

    /// <summary>
    /// Gets or sets the <see cref="Color"/> for this font.
    /// </summary>
    public Color SelectedFontColor
    {
        get => (Color)GetValue(SelectedFontColorProperty);
        set => SetValue(SelectedFontColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="FontFamily"/> for this font.
    /// </summary>
    public FontFamily SelectedFontFamily
    {
        get => (FontFamily)GetValue(SelectedFontFamilyProperty);
        set => SetValue(SelectedFontFamilyProperty, value);
    }

    /// <summary>
    /// Gets or sets the size for this font.
    /// </summary>
    public double SelectedFontSize
    {
        get => (double)GetValue(SelectedFontSizeProperty);
        set => SetValue(SelectedFontSizeProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="FontStretch"/> for this font.
    /// </summary>
    public FontStretch SelectedFontStretch
    {
        get => (FontStretch)GetValue(SelectedFontStretchProperty);
        set => SetValue(SelectedFontStretchProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="FontStyle"/> for this font.
    /// </summary>
    public FontStyle SelectedFontStyle
    {
        get => (FontStyle)GetValue(SelectedFontStyleProperty);
        set => SetValue(SelectedFontStyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the <see cref="FontWeight"/> for this font.
    /// </summary>
    public FontWeight SelectedFontWeight
    {
        get => (FontWeight)GetValue(SelectedFontWeightProperty);
        set => SetValue(SelectedFontWeightProperty, value);
    }
}
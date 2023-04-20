using System.Windows;
using System.Windows.Media;
using VectronsLibrary.Wpf.Controls;

namespace VectronsLibrary.Wpf.Dialogs;

/// <summary>
/// Interaction logic for FontPickerDialog.xaml.
/// </summary>
public partial class FontPickerDialog : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FontPickerDialog"/> class.
    /// </summary>
    public FontPickerDialog()
    {
        InitializeComponent();
        DefaultFontColor = (Color)FontPicker.SelectedFontColorProperty.DefaultMetadata.DefaultValue;
        DefaultFontFamily = (FontFamily)FontPicker.SelectedFontFamilyProperty.DefaultMetadata.DefaultValue;
        DefaultFontSize = (double)FontPicker.SelectedFontSizeProperty.DefaultMetadata.DefaultValue;
        DefaultFontStretch = (FontStretch)FontPicker.SelectedFontStretchProperty.DefaultMetadata.DefaultValue;
        DefaultFontStyle = (FontStyle)FontPicker.SelectedFontStyleProperty.DefaultMetadata.DefaultValue;
        DefaultFontWeight = (FontWeight)FontPicker.SelectedFontWeightProperty.DefaultMetadata.DefaultValue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontPickerDialog"/> class.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    public FontPickerDialog(Window parent)
        : this()
        => Owner = parent;

    /// <summary>
    /// Gets or sets the default font <see cref="Color"/>.
    /// </summary>
    public Color DefaultFontColor
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the default font <see cref="FontFamily"/>.
    /// </summary>
    public FontFamily DefaultFontFamily
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the default font size.
    /// </summary>
    public double DefaultFontSize
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the default font <see cref="FontStretch"/>.
    /// </summary>
    public FontStretch DefaultFontStretch
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the default font <see cref="FontStyle"/>.
    /// </summary>
    public FontStyle DefaultFontStyle
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the default font <see cref="FontWeight"/>.
    /// </summary>
    public FontWeight DefaultFontWeight
    {
        get;
        set;
    }

    /// <inheritdoc cref="FontPicker.SelectedFontColor"/>
    public Color SelectedFontColor
    {
        get;
        set;
    }

    /// <inheritdoc cref="FontPicker.SelectedFontFamily"/>
    public FontFamily? SelectedFontFamily
    {
        get;
        set;
    }

    /// <inheritdoc cref="FontPicker.SelectedFontSize"/>
    public double SelectedFontSize
    {
        get;
        set;
    }

    /// <inheritdoc cref="FontPicker.SelectedFontStretch"/>
    public FontStretch SelectedFontStretch
    {
        get;
        set;
    }

    /// <inheritdoc cref="FontPicker.SelectedFontStyle"/>
    public FontStyle SelectedFontStyle
    {
        get;
        set;
    }

    /// <inheritdoc cref="FontPicker.SelectedFontWeight"/>
    public FontWeight SelectedFontWeight
    {
        get;
        set;
    }

    /// <summary>
    /// Sets all values to the initial value's.
    /// </summary>
    public void ResetToStartValues()
    {
        SelectedFontColor = DefaultFontColor;
        SelectedFontFamily = DefaultFontFamily;
        SelectedFontSize = DefaultFontSize;
        SelectedFontStretch = DefaultFontStretch;
        SelectedFontStyle = DefaultFontStyle;
        SelectedFontWeight = DefaultFontWeight;
        return;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
        => DialogResult = true;
}
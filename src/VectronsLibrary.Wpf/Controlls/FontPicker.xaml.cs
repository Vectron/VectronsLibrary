using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VectronsLibrary.Wpf.Controlls
{
    /// <summary>
    /// Interaction logic for FontPicker.xaml
    /// </summary>
    public partial class FontPicker : UserControl
    {
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedFontColor),
                typeof(Color),
                typeof(FontPicker),
                new PropertyMetadata(((SolidColorBrush)ForegroundProperty.DefaultMetadata.DefaultValue).Color));

        public static readonly DependencyProperty SelectedFontFamilyProperty =
            DependencyProperty.Register(
                nameof(SelectedFontFamily),
                typeof(FontFamily),
                typeof(FontPicker),
                new PropertyMetadata(FontFamilyProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty SelectedFontSizeProperty =
            DependencyProperty.Register(
                nameof(SelectedFontSize),
                typeof(double),
                typeof(FontPicker),
                new PropertyMetadata(FontSizeProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty SelectedFontStretchProperty =
            DependencyProperty.Register(
                nameof(SelectedFontStretch),
                typeof(FontStretch),
                typeof(FontPicker),
                new PropertyMetadata(FontStretchProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty SelectedFontStyleProperty =
            DependencyProperty.Register(
                nameof(SelectedFontStyle),
                typeof(FontStyle),
                typeof(FontPicker),
                new PropertyMetadata(FontStyleProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty SelectedFontWeightProperty =
            DependencyProperty.Register(
                nameof(SelectedFontWeight),
                typeof(FontWeight),
                typeof(FontPicker),
                new PropertyMetadata(FontWeightProperty.DefaultMetadata.DefaultValue));

        public FontPicker()
        {
            InitializeComponent();
        }

        public Color SelectedFontColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public FontFamily SelectedFontFamily
        {
            get => (FontFamily)GetValue(SelectedFontFamilyProperty);
            set => SetValue(SelectedFontFamilyProperty, value);
        }

        public double SelectedFontSize
        {
            get => (double)GetValue(SelectedFontSizeProperty);
            set => SetValue(SelectedFontSizeProperty, value);
        }

        public FontStretch SelectedFontStretch
        {
            get => (FontStretch)GetValue(SelectedFontStretchProperty);
            set => SetValue(SelectedFontStretchProperty, value);
        }

        public FontStyle SelectedFontStyle
        {
            get => (FontStyle)GetValue(SelectedFontStyleProperty);
            set => SetValue(SelectedFontStyleProperty, value);
        }

        public FontWeight SelectedFontWeight
        {
            get => (FontWeight)GetValue(SelectedFontWeightProperty);
            set => SetValue(SelectedFontWeightProperty, value);
        }
    }
}
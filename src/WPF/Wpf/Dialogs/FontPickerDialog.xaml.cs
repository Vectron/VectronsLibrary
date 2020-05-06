using System.Windows;
using System.Windows.Media;
using VectronsLibrary.Wpf.Controlls;

namespace VectronsLibrary.Wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for FontPickerDialog.xaml
    /// </summary>
    public partial class FontPickerDialog : Window
    {
        public FontPickerDialog()
        {
            InitializeComponent();
            DefaultFontColor = (Color)FontPicker.SelectedColorProperty.DefaultMetadata.DefaultValue;
            DefaultFontFamily = (FontFamily)FontPicker.SelectedFontFamilyProperty.DefaultMetadata.DefaultValue;
            DefaultFontSize = (double)FontPicker.SelectedFontSizeProperty.DefaultMetadata.DefaultValue;
            DefaultFontStretch = (FontStretch)FontPicker.SelectedFontStretchProperty.DefaultMetadata.DefaultValue;
            DefaultFontStyle = (FontStyle)FontPicker.SelectedFontStyleProperty.DefaultMetadata.DefaultValue;
            DefaultFontWeight = (FontWeight)FontPicker.SelectedFontWeightProperty.DefaultMetadata.DefaultValue;
        }

        public FontPickerDialog(Window parrent)
            : this()
        {
            Owner = parrent;
        }

        public Color DefaultFontColor
        {
            get;
            set;
        }

        public FontFamily DefaultFontFamily
        {
            get;
            set;
        }

        public double DefaultFontSize
        {
            get;
            set;
        }

        public FontStretch DefaultFontStretch
        {
            get;
            set;
        }

        public FontStyle DefaultFontStyle
        {
            get;
            set;
        }

        public FontWeight DefaultFontWeight
        {
            get;
            set;
        }

        public Color SelectedFontColor
        {
            get => fontpicker.SelectedFontColor;
            set => fontpicker.SelectedFontColor = value;
        }

        public FontFamily SelectedFontFamily
        {
            get => fontpicker.SelectedFontFamily;
            set => fontpicker.SelectedFontFamily = value;
        }

        public double SelectedFontSize
        {
            get => fontpicker.SelectedFontSize;
            set => fontpicker.SelectedFontSize = value;
        }

        public FontStretch SelectedFontStretch
        {
            get => fontpicker.SelectedFontStretch;
            set => fontpicker.SelectedFontStretch = value;
        }

        public FontStyle SelectedFontStyle
        {
            get => fontpicker.SelectedFontStyle;
            set => fontpicker.SelectedFontStyle = value;
        }

        public FontWeight SelectedFontWeight
        {
            get => fontpicker.SelectedFontWeight;
            set => fontpicker.SelectedFontWeight = value;
        }

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
        {
            DialogResult = true;
        }
    }
}
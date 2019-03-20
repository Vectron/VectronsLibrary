using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace VectronsLibrary.Wpf.Controlls
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(Color),
                typeof(ColorPicker),
                new PropertyMetadata(Colors.Black));

        public ColorPicker()
        {
            InitializeComponent();
        }

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

        public IEnumerable<string> ColorPickerColors
            => typeof(Colors)
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Select(p => p.Name);

        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }
    }
}
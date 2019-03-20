using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media;

namespace VectronsLibrary.Wpf.Converters
{
    [ValueConversion(typeof(Color), typeof(string))]
    public class ColorToNameConverter : IValueConverter
    {
        private readonly ILookup<Color, string> properties;

        public ColorToNameConverter()
        {
            var colors = typeof(Colors);
            properties = colors
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .ToLookup(x => (Color)x.GetValue(null, null), x => x.Name);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colorToFind = (Color)value;
            return properties[colorToFind].FirstOrDefault() ?? value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ColorConverter.ConvertFromString((string)value);
        }
    }
}
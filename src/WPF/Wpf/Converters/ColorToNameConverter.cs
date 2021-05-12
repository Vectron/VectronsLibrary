using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media;

namespace VectronsLibrary.Wpf.Converters
{
    /// <summary>
    /// Provides a type converter to convert <see cref="FamilyTypefaceCollection"/> objects to and from <see langword="string"/>.
    /// </summary>
    [ValueConversion(typeof(Color), typeof(string))]
    public class ColorToNameConverter : IValueConverter
    {
        private readonly ILookup<Color, string> properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorToNameConverter"/> class.
        /// </summary>
        public ColorToNameConverter()
        {
            var colors = typeof(Colors);
            properties = colors
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .ToLookup(x => (Color)x.GetValue(null, null)!, x => x.Name);
        }

        /// <inheritdoc/>
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colorToFind = (Color)value;
            return properties[colorToFind].FirstOrDefault() ?? value.ToString();
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ColorConverter.ConvertFromString((string)value);
        }
    }
}
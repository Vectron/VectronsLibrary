using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VectronsLibrary.Wpf.Converters
{
    /// <summary>
    /// Provides a type converter to convert to reverse other converters.
    /// </summary>
    [ContentProperty("Converter")]
    public class ReverseConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets he underlying <see cref="IValueConverter"/> to use when converting.
        /// </summary>
        public IValueConverter? Converter
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => Converter?.ConvertBack(value, targetType, parameter, culture);

        /// <inheritdoc/>
        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Converter?.Convert(value, targetType, parameter, culture);
    }
}
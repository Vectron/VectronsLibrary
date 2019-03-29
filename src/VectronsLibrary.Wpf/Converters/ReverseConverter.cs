using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VectronsLibrary.Wpf.Converters
{
    [ContentProperty("Converter")]
    public class ReverseConverter : IValueConverter
    {
        public IValueConverter Converter
        {
            get;
            set;
        }

        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Converter.ConvertBack(value, targetType, parameter, culture);
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Converter.Convert(value, targetType, parameter, culture);
        }
    }
}
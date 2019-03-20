using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace VectronsLibrary.Wpf.Converters
{
    [ValueConversion(typeof(FamilyTypefaceCollection), typeof(object))]
    internal class FamilyTypefacesFilter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var propName = (string)parameter;
            var items = (FamilyTypefaceCollection)value;
            var propInfo = typeof(FamilyTypeface).GetProperty(propName);
            return items.Select(x => propInfo.GetValue(x)).Distinct();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
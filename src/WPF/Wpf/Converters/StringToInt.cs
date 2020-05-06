using System;
using System.Globalization;
using System.Windows.Data;

namespace VectronsLibrary.Wpf.Converters
{
    public class StringToInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string items)
            {
                if (targetType == typeof(short))
                {
                    short.TryParse(items, out short resultValue);
                    return resultValue;
                }
                else if (targetType == typeof(int))
                {
                    int.TryParse(items, out int resultValue);
                    return resultValue;
                }
                else if (targetType == typeof(long))
                {
                    long.TryParse(items, out long resultValue);
                    return resultValue;
                }
                else if (targetType == typeof(float))
                {
                    float.TryParse(items, out float resultValue);
                    return resultValue;
                }
                else if (targetType == typeof(double))
                {
                    double.TryParse(items, out double resultValue);
                    return resultValue;
                }
            }

            return 0;
        }
    }
}
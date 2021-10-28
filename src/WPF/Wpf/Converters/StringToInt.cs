using System;
using System.Globalization;
using System.Windows.Data;

namespace VectronsLibrary.Wpf.Converters;

/// <summary>
/// Provides a type converter to convert <see cref="string"/> objects to and from number types.
/// </summary>
[ValueConversion(typeof(string), typeof(object))]
public class StringToInt : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value;

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string items)
        {
            if (targetType == typeof(short))
            {
                _ = short.TryParse(items, out var resultValue);
                return resultValue;
            }
            else if (targetType == typeof(int))
            {
                _ = int.TryParse(items, out var resultValue);
                return resultValue;
            }
            else if (targetType == typeof(long))
            {
                _ = long.TryParse(items, out var resultValue);
                return resultValue;
            }
            else if (targetType == typeof(float))
            {
                _ = float.TryParse(items, out var resultValue);
                return resultValue;
            }
            else if (targetType == typeof(double))
            {
                _ = double.TryParse(items, out var resultValue);
                return resultValue;
            }
        }

        return 0;
    }
}
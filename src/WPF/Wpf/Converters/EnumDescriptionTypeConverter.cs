using System;
using System.ComponentModel;

namespace VectronsLibrary.Wpf.Converters;

/// <summary>
/// Provides a type converter to convert <see cref="Enum"/> objects to and from various other representations.
/// http://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/.
/// </summary>
public class EnumDescriptionTypeConverter : EnumConverter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumDescriptionTypeConverter"/> class.
    /// </summary>
    /// <param name="type">The type to convert.</param>
    public EnumDescriptionTypeConverter(Type type)
        : base(type)
    {
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType != typeof(string) || value == null)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }

        var fi = value.GetType().GetField(value.ToString()!);
        if (fi == null)
        {
            return string.Empty;
        }

        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return ((attributes.Length > 0) && (!string.IsNullOrEmpty(attributes[0].Description)))
            ? attributes[0].Description
            : value.ToString();
    }
}
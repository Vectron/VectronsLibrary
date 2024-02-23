using System.ComponentModel;

namespace VectronsLibrary;

/// <inheritdoc />
/// <summary>
/// Initializes a new instance of the <see cref="PropertyChangedEventArgs{T}"/> class.
/// </summary>
/// <param name="propertyName">The name of the property that changed.</param>
/// <param name="previousValue">The previous value of the property.</param>
/// <param name="currentValue">The new value of the property.</param>
public class PropertyChangedEventArgs<T>(string propertyName, T previousValue, T currentValue) : PropertyChangedEventArgs(propertyName)
{
    /// <summary>
    /// Gets the new value of the property.
    /// </summary>
    public T CurrentValue { get; private set; } = currentValue;

    /// <summary>
    /// Gets the previous value of the property.
    /// </summary>
    public T PreviousValue { get; private set; } = previousValue;
}

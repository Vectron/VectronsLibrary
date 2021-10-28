using System.ComponentModel;

namespace VectronsLibrary;

/// <inheritdoc />
public class PropertyChangedEventArgs<T> : PropertyChangedEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyChangedEventArgs{T}"/> class.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    /// <param name="previousValue">The previous value of the property.</param>
    /// <param name="currentValue">The new value of the property.</param>
    public PropertyChangedEventArgs(string propertyName, T previousValue, T currentValue)
        : base(propertyName)
    {
        PreviousValue = previousValue;
        CurrentValue = currentValue;
    }

    /// <summary>
    /// Gets the new value of the property.
    /// </summary>
    public T CurrentValue
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the previous value of the property.
    /// </summary>
    public T PreviousValue
    {
        get;
        private set;
    }
}
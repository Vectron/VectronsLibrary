using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VectronsLibrary;

/// <summary>
/// Base class to implement <see cref="INotifyPropertyChanged"/>.
/// </summary>
public abstract class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
{
    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc/>
    public event PropertyChangingEventHandler? PropertyChanging;

    /// <summary>
    /// Invoke a property changed.
    /// </summary>
    /// <param name="propertyName">Name of the property that changed.</param>
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        VerifyPropertyName(propertyName);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Invoke a property changed.
    /// </summary>
    /// <typeparam name="T">Type of the property.</typeparam>
    /// <param name="previousValue">The previous value of the property.</param>
    /// <param name="currentValue">The new value of the property.</param>
    /// <param name="propertyName">Name of the property that changed.</param>
    protected void OnPropertyChanged<T>(T previousValue, T currentValue, [CallerMemberName] string propertyName = "")
    {
        VerifyPropertyName(propertyName);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs<T>(propertyName, previousValue, currentValue));
    }

    /// <summary>
    /// Invoke a property changing.
    /// </summary>
    /// <param name="propertyName">Name of the property that changed.</param>
    protected void OnPropertyChanging([CallerMemberName] string propertyName = "")
    {
        VerifyPropertyName(propertyName);
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }

    /// <summary>
    /// Helper for setting the value on a field, and check if property changed should be invoked.
    /// </summary>
    /// <typeparam name="T">Type of the property.</typeparam>
    /// <param name="field">reference to the field that needs to be updated.</param>
    /// <param name="value">The new value of the property.</param>
    /// <param name="propertyName">Name of the property that changed.</param>
    /// <returns>Value if the field is updated or not.</returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        OnPropertyChanging(propertyName);
        var oldValue = field;
        field = value;
        OnPropertyChanged(oldValue, field, propertyName);
        return true;
    }

    /// <summary>
    /// Warns the developer if this object does not have a public property with the specified name.
    /// This method does not exist in a Release build.
    /// </summary>
    /// <param name="propertyName">The name of the property to check.</param>
    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    private void VerifyPropertyName(string propertyName = "")
    {
        if (propertyName is null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        // Verify that the property name matches a real, public, instance property on this object.
        if (TypeDescriptor.GetProperties(this)[propertyName] == null)
        {
            throw new InvalidOperationException($"Invalid property name: {propertyName}");
        }
    }
}

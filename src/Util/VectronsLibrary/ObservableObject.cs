using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VectronsLibrary
{
    /// <summary>
    /// Base class to implement <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets a value indicating whether whether an exception is thrown, or if a Debug.Fail() is used
        /// when an invalid property name is passed to the VerifyPropertyName method.
        /// The default value is false, but subclasses used by unit tests might
        /// override this property's getter to return true.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName
        {
            get;
            private set;
        }

        /// <summary>
        /// Warns the developer if this object does not have a public property with the specified name.
        /// This method does not exist in a Release build.
        /// </summary>
        /// <param name="propertyName">The name of the property to check.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public virtual void VerifyPropertyName(string? propertyName)
        {
            // Verify that the property name matches a real,
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                var msg = "Invalid property name: " + propertyName;

                if (ThrowOnInvalidPropertyName)
                {
                    throw new InvalidOperationException(msg);
                }
                else
                {
                    Debug.Fail(msg);
                }
            }
        }

        /// <summary>
        /// Invoke a property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            VerifyPropertyName(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Invoke a property changed.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="previousValue">The previous value of the propertry.</param>
        /// <param name="currentValue">The new value of the property.</param>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected void OnPropertyChanged<T>(T previousValue, T currentValue, [CallerMemberName] string? propertyName = null)
        {
            VerifyPropertyName(propertyName);
            if (propertyName == null)
            {
                return;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs<T>(propertyName, previousValue, currentValue));
        }

        /// <summary>
        /// Helper for setting the value on a field, and check if property changed should be invoked.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="field">reference to the field that needs to be updated.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="propertyName">Name of the property that changed.</param>
        /// <returns>Value if the field is updated or not.</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            var oldValue = field;
            field = value;
            OnPropertyChanged(oldValue, field, propertyName);
            return true;
        }
    }
}
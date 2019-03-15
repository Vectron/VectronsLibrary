using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VectronsLibrary
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used
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
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public virtual void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

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

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            VerifyPropertyName(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged<T>(T previousValue, T currentValue, [CallerMemberName] string propertyName = null)
        {
            VerifyPropertyName(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs<T>(propertyName, previousValue, currentValue));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
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
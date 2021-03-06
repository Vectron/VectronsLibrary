﻿using System.ComponentModel;

namespace VectronsLibrary
{
    public class PropertyChangedEventArgs<T> : PropertyChangedEventArgs
    {
        public PropertyChangedEventArgs(string propertyName, T previousValue, T currentValue)
            : base(propertyName)
        {
            PreviousValue = previousValue;
            CurrentValue = currentValue;
        }

        public T CurrentValue
        {
            get; private set;
        }

        public T PreviousValue
        {
            get; private set;
        }
    }
}
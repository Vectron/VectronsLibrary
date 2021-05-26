using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VectronsLibrary.Tests
{
    [TestClass]
    public class ObservableObjectTests
    {
        [TestMethod]
        public void DoesNotThrowWhenNoPropertyChangedEventRegistered()
        {
            var test = new ObservableObjectTestClass();
            test.InvokeOnPropertyChanging(nameof(ObservableObjectTestClass.TestField));
        }

        [TestMethod]
        public void DoesNotThrowWhenNoPropertyChangingEventRegistered()
        {
            var test = new ObservableObjectTestClass();
            test.InvokeOnPropertyChanged(nameof(ObservableObjectTestClass.TestField));
            test.InvokeOnPropertyChanged(0, 1, nameof(ObservableObjectTestClass.TestField));
        }

        [TestMethod]
        public void EventsNotFiredWhenNoChange()
        {
            var test = new ObservableObjectTestClass();
            test.PropertyChanging += (s, e) => Assert.Fail();
            test.PropertyChanged += (s, e) => Assert.Fail();
            test.TestField = test.TestField;
        }

        [TestMethod]
        public void FieldIsUpdated()
        {
            var test = new ObservableObjectTestClass();
            Assert.AreEqual(0, test.TestField);
            test.TestField = 2;
            Assert.AreEqual(2, test.TestField);
        }

        [TestMethod]
        public void PropertyChangedContainsPropertyNameOldValueAndNewValue()
        {
            var test = new ObservableObjectTestClass();
            PropertyChangedEventArgs<int>? recordedEvents = null;
            var oldValue = test.TestField;
            var newValue = 2;
            test.PropertyChanged += (s, e) => recordedEvents = e as PropertyChangedEventArgs<int>;
            test.TestField = newValue;

            Assert.IsNotNull(recordedEvents);
            Assert.AreEqual(oldValue, recordedEvents.PreviousValue);
            Assert.AreEqual(newValue, recordedEvents.CurrentValue);
            Assert.AreEqual(nameof(ObservableObjectTestClass.TestField), recordedEvents.PropertyName);
        }

        [TestMethod]
        public void PropertyChangedEventIsInvoked()
        {
            var test = new ObservableObjectTestClass();
            var recordedEvents = new List<PropertyChangedEventArgs>();
            test.PropertyChanged += (s, e) =>
            {
                Assert.AreSame(test, s);
                recordedEvents.Add(e);
            };
            test.InvokeOnPropertyChanged(nameof(ObservableObjectTestClass.TestField));
            test.InvokeOnPropertyChanged(0, 1, nameof(ObservableObjectTestClass.TestField));
            Assert.AreEqual(2, recordedEvents.Count);
            Assert.IsInstanceOfType(recordedEvents[1], typeof(PropertyChangedEventArgs<int>));
        }

        [TestMethod]
        public void PropertyChangingEventIsInvoked()
        {
            var test = new ObservableObjectTestClass();
            var recordedEvents = new List<PropertyChangingEventArgs>();
            test.PropertyChanging += (s, e) =>
            {
                Assert.AreSame(test, s);
                recordedEvents.Add(e);
            };
            test.InvokeOnPropertyChanging(nameof(ObservableObjectTestClass.TestField));
            Assert.AreEqual(1, recordedEvents.Count);
        }

        [TestMethod]
        public void PropertyChaningIsCalledBeforePropertyChanged()
        {
            var test = new ObservableObjectTestClass();
            var recordedEvents = new List<EventArgs>();
            test.PropertyChanging += (s, e) =>
            {
                Assert.AreSame(test, s);
                recordedEvents.Add(e);
            };
            test.PropertyChanged += (s, e) =>
            {
                Assert.AreSame(test, s);
                recordedEvents.Add(e);
            };
            test.TestField = 2;
            Assert.AreEqual(2, recordedEvents.Count);
            Assert.IsInstanceOfType(recordedEvents[0], typeof(PropertyChangingEventArgs));
            Assert.IsInstanceOfType(recordedEvents[1], typeof(PropertyChangedEventArgs<int>));
        }

        [TestMethod]
        public void PropertyChaningIsCalledBeforePropertyChangedFieldIsUpdatedInbetween()
        {
            var test = new ObservableObjectTestClass();
            var recordedEvents = new List<EventArgs>();
            var oldValue = test.TestField;
            var newValue = 2;
            test.PropertyChanging += (s, e) =>
            {
                Assert.AreSame(test, s);
                Assert.AreEqual(oldValue, test.TestField);
                recordedEvents.Add(e);
            };
            test.PropertyChanged += (s, e) =>
            {
                Assert.AreSame(test, s);
                Assert.AreEqual(newValue, test.TestField);
                recordedEvents.Add(e);
            };
            test.TestField = newValue;
            Assert.AreEqual(2, recordedEvents.Count);
            Assert.IsInstanceOfType(recordedEvents[0], typeof(PropertyChangingEventArgs));
            Assert.IsInstanceOfType(recordedEvents[1], typeof(PropertyChangedEventArgs<int>));
        }

        [TestMethod]
        [Conditional("DEBUG")]
        public void ThrowIfNameIsnull()
        {
            var test = new ObservableObjectTestClass();
            _ = Assert.ThrowsException<ArgumentNullException>(() => test.InvokeOnPropertyChanged(null));
            _ = Assert.ThrowsException<ArgumentNullException>(() => test.InvokeOnPropertyChanged(0, 1, null));
        }

        [TestMethod]
        [Conditional("DEBUG")]
        public void ThrowsInvalidOperationExceptionIfPropertyDoesNotExcist()
        {
            var test = new ObservableObjectTestClass();
            _ = Assert.ThrowsException<InvalidOperationException>(() => test.InvokeOnPropertyChanged(string.Empty));
            _ = Assert.ThrowsException<InvalidOperationException>(() => test.InvokeOnPropertyChanged(0, 1, string.Empty));
        }

        private class ObservableObjectTestClass : ObservableObject
        {
            private int testField;

            public int TestField
            {
                get => testField;
                set => SetField(ref testField, value);
            }

            public void InvokeOnPropertyChanged(string? propertyName)
            {
                OnPropertyChanged(propertyName!);
            }

            public void InvokeOnPropertyChanged<T>(T previousValue, T currentValue, string? propertyName)
            {
                OnPropertyChanged(previousValue, currentValue, propertyName!);
            }

            public void InvokeOnPropertyChanging(string? propertyName)
            {
                OnPropertyChanging(propertyName!);
            }
        }
    }
}
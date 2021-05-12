using System;
using System.Collections.Generic;

namespace VectronsLibrary.DI
{
    /// <summary>
    /// A collection of all the types that inherit from <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The base type to look for.</typeparam>
    public interface IRegisteredTypes<T>
    {
        /// <summary>
        /// Gets the list of items that inherit from <typeparamref name="T"/>.
        /// </summary>
        IEnumerable<Type> Items
        {
            get;
        }
    }
}
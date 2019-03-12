using System;
using System.Collections.Generic;

namespace VectronsLibrary.DI
{
    public interface IRegisteredTypes<T>
    {
        IEnumerable<Type> Items
        {
            get;
            set;
        }
    }
}
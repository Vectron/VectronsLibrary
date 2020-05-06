using System;
using System.Collections.Generic;
using System.Text;

namespace VectronsLibrary.DI.Abstraction
{
    public interface IFactory<T>
        where T : class
    {
        T Value
        {
            get;
        }

        IEnumerable<string> GetItemNames();

        T GetValue(string name);
    }
}

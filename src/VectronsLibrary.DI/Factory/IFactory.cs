using System.Collections.Generic;

namespace VectronsLibrary.DI.Factory
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
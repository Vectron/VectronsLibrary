using System;

namespace VectronsLibrary.DI
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class SingletonAttribute : Attribute
    {
        public SingletonAttribute()
        {
        }
    }
}
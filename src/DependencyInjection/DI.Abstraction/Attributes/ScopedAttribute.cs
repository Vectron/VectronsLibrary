using System;

namespace VectronsLibrary.DI
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ScopedAttribute : Attribute
    {
        public ScopedAttribute()
        {
        }
    }
}
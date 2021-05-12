using System;

namespace VectronsLibrary.DI
{
    /// <summary>
    /// Attribute indicating the type should be skipped by autoimatic dependincy scanning.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class IgnoreAttribute : Attribute
    {
    }
}
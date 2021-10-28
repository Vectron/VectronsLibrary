using System;

namespace VectronsLibrary.DI;

/// <summary>
/// Attribute indicating the type should be registered as singleton by autoimatic dependincy scanning.
/// </summary>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class SingletonAttribute : Attribute
{
}
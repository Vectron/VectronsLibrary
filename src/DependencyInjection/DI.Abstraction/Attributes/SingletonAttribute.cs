using System;

namespace VectronsLibrary.DI.Attributes;

/// <summary>
/// Attribute indicating the type should be registered as singleton by automatic dependency scanning.
/// </summary>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class SingletonAttribute : Attribute
{
}
